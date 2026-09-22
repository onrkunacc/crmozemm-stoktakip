using HospitalInventoryManagement.BL.Extensions;
using HospitalInventoryManagement.BL.Interfaces;
using HospitalInventoryManagement.Data.Context;
using HospitalInventoryManagement.Data.DTOs;
using HospitalInventoryManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalInventoryManagement.BL.Service
{
    public class StockService : IStockService
    {
        private readonly ApplicationDbContext _context;

        public StockService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Stock> GetAllStocks()
        {
            return _context.Stocks.Include(s => s.Product)
                                  .Include(s => s.Hospital)
                                  .ToList();
        }

        public List<Stock> GetStocksByHospital(int hospitalId)
        {
            return _context.Stocks.Include(s => s.Product)
                                  .Include(s => s.Hospital)
                                  .Where(s => s.HospitalID == hospitalId)
                                  .ToList();
        }

        public void UpdateStock(Stock stock)
        {
            var existingStock = _context.Stocks.FirstOrDefault(s => s.ProductID == stock.ProductID && s.HospitalID == stock.HospitalID);

            if (existingStock != null)
            {
                existingStock.Quantity = stock.Quantity;
                existingStock.ProductID = stock.ProductID;
                existingStock.HospitalID = stock.HospitalID;
                existingStock.ExpiryDate = stock.ExpiryDate;
                existingStock.LastUpdated = stock.LastUpdated;

                _context.SaveChanges();
            }
        }

        public void DeleteStock(int stockId)
        {
            var stock = _context.Stocks.Find(stockId);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
                _context.SaveChanges();
            }
        }
        public void AddStock(Stock stock)
        {
            _context.Stocks.Add(stock);
            _context.SaveChanges();
        }

        public Stock GetStockById(int id)
        {
            return _context.Stocks
                     .Include(s => s.Product) // Product ilişkisinin dahil edilmesi
                     .Include(s => s.Hospital) // Hospital ilişkisinin dahil edilmesi
                     .FirstOrDefault(s => s.StockID == id);
        }
        public void AddStockTransaction(StockTransaction transaction)
        {
            _context.StockTransactions.Add(transaction);
            _context.SaveChanges();
        }

        public void UpdateStockQuantity(int stockId, int changeQuantity)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.StockID == stockId);
            if (stock != null)
            {
                stock.Quantity += changeQuantity;
                _context.SaveChanges();
            }
        }

        public Stock GetStockByReferenceNumber(string referenceNumber, int hospitalId)
        {
            return _context.Stocks
                           .Include(s => s.Product)
                           .Include(s => s.Hospital)
                           .FirstOrDefault(s => s.Product.ReferenceNumber == referenceNumber && s.HospitalID == hospitalId);
        }


        public bool ProcessStockTransaction(string referenceNumber, int quantityChange, int hospitalId, string lotNumber = null, DateTime? expiryDate = null)
        {
            // Stoğu referans numarası ve hastane ID'sine göre bul
            var stock = _context.Stocks
                                .Include(s => s.Product)
                                .FirstOrDefault(s => s.Product.ReferenceNumber == referenceNumber && s.HospitalID == hospitalId);

            if (stock == null)
            {
                // Stoğun bulunamaması durumunda yeni stok kaydı oluştur
                stock = new Stock
                {
                    ProductID = _context.Products.FirstOrDefault(p => p.ReferenceNumber == referenceNumber)?.ProductID ?? 0,
                    HospitalID = hospitalId,
                    Quantity = quantityChange, // İlk stok miktarı olarak giriş miktarını ayarla
                    LotNumber = lotNumber,
                    ExpiryDate = expiryDate ?? DateTime.Now,
                    LastUpdated = DateTime.Now
                };

                _context.Stocks.Add(stock); // Yeni stok kaydını ekle
            }
            else
            {
                // Stok çıkışı için miktar yeterliliğini kontrol et
                if (quantityChange < 0 && stock.Quantity + quantityChange < 0)
                {
                    throw new InvalidOperationException("Yetersiz stok miktarı. Çıkış yaparken stok miktarı 0'ın altına düşemez.");
                }

                // Mevcut stok kaydını güncelle
                stock.Quantity += quantityChange;
                stock.LotNumber = lotNumber ?? stock.LotNumber; // Eğer yeni lot numarası varsa güncelle
                stock.ExpiryDate = expiryDate ?? stock.ExpiryDate; // Eğer yeni bir son kullanma tarihi varsa güncelle
                stock.LastUpdated = DateTime.Now;
            }

            // İşlem kaydını `StockTransaction` tablosuna ekle
            var transaction = new StockTransaction
            {
                StockID = stock.StockID,
                ChangeQuantity = quantityChange,
                TransactionDate = DateTime.Now,
                TransactionType = quantityChange > 0 ? "Giriş" : "Çıkış",
                LotNumber = lotNumber, // Lot numarasını işlem kaydına ekliyoruz
                Description = quantityChange > 0 ? "Barkod ile Stok Girişi" : "Barkod ile Stok Çıkışı"
            };

            // İşlem kaydını veritabanına ekle ve tüm değişiklikleri kaydet
            _context.StockTransactions.Add(transaction);
            _context.SaveChanges();

            return true; // İşlem başarılı
        }

        public void AddNewStock(string referenceNumber, string productName, int quantity, int hospitalId, string lotNumber, DateTime expiryDate)
        {
            // Ürün tablosunda mevcutsa ürünü getir veya yeni bir ürün oluştur
            var product = _context.Products.FirstOrDefault(p => p.ReferenceNumber == referenceNumber)
                          ?? new Product { ReferenceNumber = referenceNumber, ProductName = productName };

            // Yeni stok oluştur
            var newStock = new Stock
            {
                Product = product,
                HospitalID = hospitalId,
                Quantity = quantity,
                LotNumber = lotNumber,
                ExpiryDate = expiryDate,
                LastUpdated = DateTime.Now
            };

            _context.Stocks.Add(newStock);
            _context.SaveChanges();
        }



        public StockCountResultDTO ApplyCount(List<StockCountLineDTO> lines, int hospitalId, string userId)
        {
            var result = new StockCountResultDTO();
            if (lines == null || lines.Count == 0)
                return result;

            var touchedProducts = new HashSet<int>();
            // Bu batch içinde açılan yeni stokları hatırla (aynı ürün+lot ikinci kez gelirse)
            var pending = new Dictionary<string, Stock>();

            using var tx = _context.Database.BeginTransaction();
            try
            {
                // Bu hastanenin AKTİF sayımı (yoksa Sayım #1 açılır). Satırlar bu sayıma yazılır.
                var sayim = GetOrCreateActiveSayim(hospitalId, userId);

                foreach (var line in lines)
                {
                    if (line == null || string.IsNullOrWhiteSpace(line.ReferenceNumber) || line.Quantity <= 0)
                        continue;

                    var product = _context.Products
                                          .FirstOrDefault(p => p.ReferenceNumber == line.ReferenceNumber);
                    if (product == null)
                    {
                        result.Warnings.Add($"Ürün bulunamadı: {line.ReferenceNumber}");
                        continue;
                    }
                    if (product.BoxTextCount <= 0)
                    {
                        result.Warnings.Add($"'{product.ProductName}' için Kutu Test Adedi tanımlı değil; satır atlandı.");
                        continue;
                    }

                    int tests = UnitConverter.ToTests(line.Quantity, line.Unit, product);
                    if (tests <= 0) continue;

                    var lot = (line.LotNumber ?? "").Trim();
                    string key = product.ProductID + "|" + lot;

                    // 1) Bu batch'te az önce açtıysak onu kullan
                    Stock stock;
                    if (!pending.TryGetValue(key, out stock))
                    {
                        // 2) Bu SAYIMDA ürün+lot var mı? (aktif sayıma bağlı)
                        stock = _context.Stocks.FirstOrDefault(s =>
                            s.SayimID == sayim.SayimID &&
                            s.ProductID == product.ProductID &&
                            (s.LotNumber ?? "") == lot);

                        if (stock == null)
                        {
                            // 3) Yoksa bu sayımda yeni satır aç (henüz SaveChanges yok)
                            stock = new Stock
                            {
                                ProductID = product.ProductID,
                                HospitalID = hospitalId,
                                SayimID = sayim.SayimID,
                                TestCount = 0,
                                LotNumber = string.IsNullOrEmpty(lot) ? null : lot,
                                ExpiryDate = line.ExpiryDate ?? DateTime.Now,
                                LastUpdated = DateTime.Now
                            };
                            _context.Stocks.Add(stock);
                        }
                        pending[key] = stock;
                    }

                    stock.TestCount += tests;
                    if (line.ExpiryDate.HasValue) stock.ExpiryDate = line.ExpiryDate.Value;
                    stock.LastUpdated = DateTime.Now;
                    SyncDerived(stock, product);

                    // İşlem kaydı: Stock'a NAVIGATION ile bağlı -> StockID otomatik dolar
                    _context.StockTransactions.Add(new StockTransaction
                    {
                        Stock = stock,
                        ChangeQuantity = line.Quantity,
                        QuantityInTests = tests,
                        Unit = line.Unit,
                        TransactionDate = DateTime.Now,
                        TransactionType = "Giriş",
                        LotNumber = lot,                 // boş string olabilir, null değil
                        Description = "Barkod sayımı ile giriş",
                        UserId = userId,
                        HospitalID = hospitalId
                    });

                    result.LineCount++;
                    result.TotalTests += tests;
                    touchedProducts.Add(product.ProductID);
                }

                _context.SaveChanges(); // TEK sefer: tüm stoklar + işlem kayıtları
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw; // gerçek hata controller'daki catch'e gider
            }

            result.ProductCount = touchedProducts.Count;
            return result;
        }
        // ---- Sayım Dönemi (oturum) ----

        // Hastanenin açık/aktif sayımı (yoksa null)
        public Sayim GetActiveSayim(int hospitalId)
        {
            return _context.Sayims.FirstOrDefault(z => z.HospitalID == hospitalId && z.IsActive);
        }

        // Aktif sayım yoksa Sayım #1 açar
        public Sayim GetOrCreateActiveSayim(int hospitalId, string userId)
        {
            var active = _context.Sayims.FirstOrDefault(z => z.HospitalID == hospitalId && z.IsActive);
            if (active != null) return active;

            int nextNo = (_context.Sayims.Where(z => z.HospitalID == hospitalId).Max(z => (int?)z.SayimNo) ?? 0) + 1;
            active = new Sayim
            {
                HospitalID = hospitalId,
                SayimNo = nextNo,
                CreatedDate = DateTime.Now,
                CreatedByUserId = userId,
                IsActive = true
            };
            _context.Sayims.Add(active);
            _context.SaveChanges();
            return active;
        }

        // Yeni sayım başlat: mevcut aktif(ler)i kapat, No+1 ile yeni aktif aç
        public Sayim StartNewSayim(int hospitalId, string userId)
        {
            var actives = _context.Sayims.Where(z => z.HospitalID == hospitalId && z.IsActive).ToList();
            foreach (var a in actives) { a.IsActive = false; a.ClosedDate = DateTime.Now; }

            int nextNo = (_context.Sayims.Where(z => z.HospitalID == hospitalId).Max(z => (int?)z.SayimNo) ?? 0) + 1;
            var yeni = new Sayim
            {
                HospitalID = hospitalId,
                SayimNo = nextNo,
                CreatedDate = DateTime.Now,
                CreatedByUserId = userId,
                IsActive = true
            };
            _context.Sayims.Add(yeni);
            _context.SaveChanges();
            return yeni;
        }

        // TestCount tek doğruluk kaynağı; Quantity (kutu) ve FlaconCount ondan türetilir.
        private void SyncDerived(Stock stock, Product product)
        {
            if (product == null || product.BoxTextCount <= 0)
                return;

            stock.Quantity = UnitConverter.ToBoxes(stock.TestCount, product);
            stock.FlaconCount = UnitConverter.ToFlacons(stock.TestCount, product);
        }

        // ---- Hastaneler arası aktarım ----

        public int GetTestCount(string referenceNumber, int hospitalId)
        {
            var sayim = GetActiveSayim(hospitalId);
            if (sayim == null) return 0;

            return _context.Stocks
                .Where(s => s.SayimID == sayim.SayimID && s.Product.ReferenceNumber == referenceNumber)
                .Select(s => (int?)s.TestCount)
                .FirstOrDefault() ?? 0;
        }

        public TransferResultDTO TransferBetweenHospitals(string referenceNumber, int quantity, string unit,
            int sourceHospitalId, int destinationHospitalId, string note, string userId)
        {
            var result = new TransferResultDTO();

            if (string.IsNullOrWhiteSpace(referenceNumber) || quantity <= 0)
            {
                result.Message = "Geçerli bir referans ve miktar girin.";
                return result;
            }
            if (sourceHospitalId == destinationHospitalId)
            {
                result.Message = "Kaynak ve hedef hastane aynı olamaz.";
                return result;
            }

            var product = _context.Products.FirstOrDefault(p => p.ReferenceNumber == referenceNumber);
            if (product == null)
            {
                result.Message = $"Ürün bulunamadı: {referenceNumber}";
                return result;
            }
            if (product.BoxTextCount <= 0)
            {
                result.Message = $"'{product.ProductName}' için Kutu Test Adedi tanımlı değil; aktarım yapılamaz.";
                return result;
            }

            int tests = UnitConverter.ToTests(quantity, unit, product);
            if (tests <= 0)
            {
                result.Message = "Aktarılacak test miktarı hesaplanamadı.";
                return result;
            }

            var sourceHospital = _context.Hospitals.FirstOrDefault(h => h.HospitalID == sourceHospitalId);
            var destHospital = _context.Hospitals.FirstOrDefault(h => h.HospitalID == destinationHospitalId);
            if (sourceHospital == null || destHospital == null)
            {
                result.Message = "Kaynak veya hedef hastane bulunamadı.";
                return result;
            }

            // Kaynak: hastanenin AKTİF sayımındaki stok
            var srcSayim = GetActiveSayim(sourceHospitalId);
            var sourceStock = srcSayim == null ? null : _context.Stocks
                .FirstOrDefault(s => s.SayimID == srcSayim.SayimID && s.ProductID == product.ProductID);

            if (sourceStock == null || sourceStock.TestCount < tests)
            {
                int available = sourceStock?.TestCount ?? 0;
                result.Message = $"Yetersiz stok. {sourceHospital.HospitalName} mevcut: {available} test, istenen: {tests} test.";
                return result;
            }

            using var dbTx = _context.Database.BeginTransaction();
            try
            {
                // Kaynaktan düş
                sourceStock.TestCount -= tests;
                sourceStock.LastUpdated = DateTime.Now;
                SyncDerived(sourceStock, product);

                // Hedef: hedefin AKTİF sayımına ekle (yoksa oluştur)
                var dstSayim = GetOrCreateActiveSayim(destinationHospitalId, userId);
                var destStock = _context.Stocks
                    .FirstOrDefault(s => s.SayimID == dstSayim.SayimID && s.ProductID == product.ProductID);

                if (destStock == null)
                {
                    destStock = new Stock
                    {
                        ProductID = product.ProductID,
                        HospitalID = destinationHospitalId,
                        SayimID = dstSayim.SayimID,
                        TestCount = tests,
                        LotNumber = sourceStock.LotNumber,
                        ExpiryDate = sourceStock.ExpiryDate,
                        LastUpdated = DateTime.Now
                    };
                    SyncDerived(destStock, product);
                    _context.Stocks.Add(destStock);
                }
                else
                {
                    destStock.TestCount += tests;
                    destStock.LastUpdated = DateTime.Now;
                    SyncDerived(destStock, product);
                }

                _context.SaveChanges(); // StockID'ler kesinleşsin

                string suffix = string.IsNullOrWhiteSpace(note) ? "" : $" | Not: {note.Trim()}";

                var outTx = new StockTransaction
                {
                    StockID = sourceStock.StockID,
                    ChangeQuantity = -quantity,
                    QuantityInTests = -tests,
                    Unit = unit,
                    TransactionDate = DateTime.Now,
                    TransactionType = "Aktarım-Çıkış",
                    LotNumber = sourceStock.LotNumber,
                    Description = $"{destHospital.HospitalName} hastanesine aktarım{suffix}",
                    UserId = userId,
                    HospitalID = sourceHospitalId
                };
                var inTx = new StockTransaction
                {
                    StockID = destStock.StockID,
                    ChangeQuantity = quantity,
                    QuantityInTests = tests,
                    Unit = unit,
                    TransactionDate = DateTime.Now,
                    TransactionType = "Aktarım-Giriş",
                    LotNumber = sourceStock.LotNumber,
                    Description = $"{sourceHospital.HospitalName} hastanesinden aktarım{suffix}",
                    UserId = userId,
                    HospitalID = destinationHospitalId
                };

                _context.StockTransactions.Add(outTx);
                _context.StockTransactions.Add(inTx);
                _context.SaveChanges(); // ID'ler üretilsin

                // Karşılıklı eşleme
                outTx.RelatedTransactionID = inTx.TransactionID;
                inTx.RelatedTransactionID = outTx.TransactionID;
                _context.SaveChanges();

                dbTx.Commit();

                result.Success = true;
                result.TransferredTests = tests;
                result.Message = $"{UnitConverter.Describe(tests, product)} " +
                                 $"{sourceHospital.HospitalName} → {destHospital.HospitalName} aktarıldı.";
                return result;
            }
            catch (Exception ex)
            {
                dbTx.Rollback();
                Console.WriteLine($"Aktarım hatası: {ex.Message}");
                result.Message = "Aktarım sırasında bir hata oluştu; işlem geri alındı.";
                return result;
            }
        }
    }
}