using HospitalInventoryManagement.BL.Extensions;
using HospitalInventoryManagement.BL.Interfaces;
using HospitalInventoryManagement.Data.Context;
using HospitalInventoryManagement.Data.DTOs;
using HospitalInventoryManagement.Data.Models;
using HospitalInventoryManagement.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using X.PagedList;
using X.PagedList.Extensions;

namespace HospitalInventoryManagement.Web.Controllers
{
    public class StockController : Controller
    {
        private readonly IStockService _stockService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public StockController(IStockService stockService, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _stockService = stockService;
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        // ================================================================
        // StockController.cs içindeki MEVCUT Index metodunun yerine bunu koy.
        // (Yalnızca Index değişiyor; sortOrder parametresi ve sıralama eklendi.)
        // ================================================================

        public async Task<IActionResult> Index(string searchQuery, int? hospitalId, int? sayimNo, int page = 1, int pageSize = 20, string sortOrder = null)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }

                if (pageSize != 20 && pageSize != 50 && pageSize != 100) pageSize = 20;
                if (page < 1) page = 1;

                var stocksQuery = _context.Stocks
                        .Include(s => s.Product).ThenInclude(p => p.Category)
                        .Include(s => s.Product.ProductType)
                        .Include(s => s.Product.Platform)
                        .Include(s => s.Hospital)
                        .Include(s => s.Sayim)
                        .AsQueryable();

                if (hospitalId.HasValue && hospitalId.Value > 0)
                {
                    stocksQuery = stocksQuery.Where(s => s.HospitalID == hospitalId.Value);
                }

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    stocksQuery = stocksQuery.Where(s => s.Product.ProductName.Contains(searchQuery));
                }

                // Sayım filtresi: seçilen sayım numarası; yoksa güncel (aktif) sayım
                if (sayimNo.HasValue && sayimNo.Value > 0)
                    stocksQuery = stocksQuery.Where(s => s.Sayim.SayimNo == sayimNo.Value);
                else
                    stocksQuery = stocksQuery.Where(s => s.Sayim.IsActive);

                // Sıralama (başlık tıklamalarından gelir)
                stocksQuery = sortOrder switch
                {
                    "name_asc" => stocksQuery.OrderBy(s => s.Product.ProductName),
                    "name_desc" => stocksQuery.OrderByDescending(s => s.Product.ProductName),
                    "category_asc" => stocksQuery.OrderBy(s => s.Product.Category.CategoryName),
                    "category_desc" => stocksQuery.OrderByDescending(s => s.Product.Category.CategoryName),
                    "ref_asc" => stocksQuery.OrderBy(s => s.Product.ReferenceNumber),
                    "ref_desc" => stocksQuery.OrderByDescending(s => s.Product.ReferenceNumber),
                    "lot_asc" => stocksQuery.OrderBy(s => s.LotNumber),
                    "lot_desc" => stocksQuery.OrderByDescending(s => s.LotNumber),
                    "hospital_asc" => stocksQuery.OrderBy(s => s.Hospital.HospitalName),
                    "hospital_desc" => stocksQuery.OrderByDescending(s => s.Hospital.HospitalName),
                    "test_asc" => stocksQuery.OrderBy(s => s.TestCount),
                    "test_desc" => stocksQuery.OrderByDescending(s => s.TestCount),
                    "skt_asc" => stocksQuery.OrderBy(s => s.ExpiryDate),
                    "skt_desc" => stocksQuery.OrderByDescending(s => s.ExpiryDate),
                    "updated_asc" => stocksQuery.OrderBy(s => s.LastUpdated),
                    "updated_desc" => stocksQuery.OrderByDescending(s => s.LastUpdated),
                    _ => stocksQuery.OrderBy(s => s.Hospital.HospitalName).ThenBy(s => s.Product.ProductName)
                };

                var pagedStocks = stocksQuery.ToPagedList(page, pageSize);

                ViewBag.SearchQuery = searchQuery;
                ViewBag.SelectedHospitalId = hospitalId;
                ViewBag.PageSize = pageSize;
                ViewBag.SortOrder = sortOrder;
                ViewBag.SayimNo = sayimNo;

                var sayimQuery = _context.Sayims.AsQueryable();
                if (hospitalId.HasValue && hospitalId.Value > 0)
                    sayimQuery = sayimQuery.Where(z => z.HospitalID == hospitalId.Value);
                ViewBag.SayimNos = sayimQuery.Select(z => z.SayimNo).Distinct().OrderBy(n => n).ToList();

                if (hospitalId.HasValue && hospitalId.Value > 0)
                {
                    var sonSayim = _context.Sayims
                        .Where(z => z.HospitalID == hospitalId.Value)
                        .OrderByDescending(z => z.SayimNo)
                        .FirstOrDefault();
                    if (sonSayim != null)
                    {
                        ViewBag.SonSayimNo = sonSayim.SayimNo;
                        ViewBag.SonSayimTarih = sonSayim.CreatedDate.ToString("dd.MM.yyyy HH:mm");
                    }
                }
                ViewBag.Hospitals = new SelectList(
                    _context.Hospitals.OrderBy(h => h.HospitalName), "HospitalID", "HospitalName", hospitalId);

                return View(pagedStocks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = "Stokları yüklerken bir hata oluştu." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Kullanıcının hastane ID'sine göre ürün listesini dolduruyoruz
            var viewModel = new StockViewModel
            {
                Products = new SelectList(_context.Products, "ProductID", "ProductName")
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StockViewModel viewModel)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // ModelState'ten Products, Hospitals, LotNumber ve ReferenceNumber alanlarını kaldırıyoruz
            ModelState.Remove("Products");
            ModelState.Remove("Hospitals");
            ModelState.Remove("LotNumber");
            ModelState.Remove("ReferenceNumber");

            // ModelState hatalarını loglama
            if (!ModelState.IsValid)
            {
                viewModel.Products = new SelectList(_context.Products, "ProductID", "ProductName");
                return View(viewModel);
            }

            try
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductID == viewModel.ProductID);
                // Ürün seçimine göre `ReferenceNumber`'ı otomatik olarak alıyoruz
                var referenceNumber = _context.Products
                                             .Where(p => p.ProductID == viewModel.ProductID)
                                             .Select(p => p.ReferenceNumber)
                                             .FirstOrDefault();

                if (string.IsNullOrEmpty(referenceNumber))
                {
                    ModelState.AddModelError("", "Ürüne ait referans numarası bulunamadı.");
                    viewModel.Products = new SelectList(_context.Products, "ProductID", "ProductName");
                    return View(viewModel);
                }

                // Yeni stok kaydı oluşturuyoruz
                var stock = new Stock
                {
                    ProductID = viewModel.ProductID,
                    HospitalID = user.HospitalID,
                    ExpiryDate = viewModel.ExpirationDate,
                    Quantity = viewModel.Quantity,
                    LotNumber = viewModel.LotNumber,
                    LastUpdated = DateTime.Now,
                    FlaconCount = product.FlaconCountPerBox
                };

                // Stok ekleme işlemi
                _stockService.AddStock(stock);
                TempData["SuccessMessage"] = "Stok girişi başarılı bir şekilde yapıldı.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                ModelState.AddModelError("", "Stok oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.");
                viewModel.Products = new SelectList(_context.Products, "ProductID", "ProductName");
                return View(viewModel);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var stock = _stockService.GetStockById(id);
            if (stock == null || (!User.IsInRole("Admin") && stock.HospitalID != user.HospitalID))
            {
                return NotFound();
            }

            var model = new StockViewModel
            {
                StockID = stock.StockID,
                ProductID = stock.ProductID,
                HospitalID = stock.HospitalID,
                Quantity = stock.Quantity,
                ExpirationDate = stock.ExpiryDate
            };

            PopulateDropdowns(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StockViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || (!User.IsInRole("Admin") && model.HospitalID != user.HospitalID))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // ModelState'ten Products ve Hospitals alanlarını kaldırdım
            ModelState.Remove("Products");
            ModelState.Remove("Hospitals");
            ModelState.Remove("LotNumber");
            ModelState.Remove("ReferenceNumber");

            // ModelState hata loglama
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Hatası: {error.ErrorMessage}");
                }

                PopulateDropdowns(model);
                return View(model);
            }

            try
            {
                var stock = _stockService.GetStockById(model.StockID);
                if (stock == null)
                {
                    ModelState.AddModelError("", "Stok bulunamadı.");
                    PopulateDropdowns(model);
                    return View(model);
                }

                Console.WriteLine($"Eski Quantity: {stock.Quantity}, Yeni Quantity: {model.Quantity}");

                // Güncelleme işlemi
                stock.StockID = model.StockID;
                stock.ProductID = model.ProductID;
                stock.HospitalID = model.HospitalID;
                stock.Quantity = model.Quantity;
                stock.ExpiryDate = model.ExpirationDate;
                stock.LastUpdated = DateTime.Now;

                _stockService.UpdateStock(stock);

                var updatedStock = _stockService.GetStockById(model.StockID);
                Console.WriteLine($"Güncellenmiş Quantity: {updatedStock.Quantity}");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                ModelState.AddModelError("", "Stok güncellenirken bir hata oluştu. Lütfen tekrar deneyin.");
                PopulateDropdowns(model);
                return View(model);
            }
        }
        [HttpGet]
        public JsonResult GetProductDetails(int productId)
        {
            var product = _context.Products
                            .Where(p => p.ProductID == productId)
                            .Select(p => new {
                                referenceNumber = p.ReferenceNumber
                            })
                            .FirstOrDefault();

            return Json(product);
        }

        private void PopulateDropdowns(StockViewModel model)
        {
            model.Products = new SelectList(_context.Products, "ProductID", "ProductName", model.ProductID);
            model.Hospitals = new SelectList(_context.Hospitals, "HospitalID", "HospitalName", model.HospitalID);
        }

        [HttpGet]
        public async Task<IActionResult> SelectStock()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var stocks = User.IsInRole("Admin")
                ? _stockService.GetAllStocks()
                : _stockService.GetStocksByHospital(user.HospitalID);

            ViewBag.Stocks = new SelectList(stocks, "StockID", "Product.ProductName");
            return View();
        }

        // Seçilen stoğa göre düzenleme sayfasına yönlendirme
        [HttpPost]
        public async Task<IActionResult> SelectStock(int stockId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var stock = _stockService.GetStockById(stockId);
            if (stock == null || (!User.IsInRole("Admin") && stock.HospitalID != user.HospitalID))
            {
                ModelState.AddModelError("", "Lütfen geçerli bir stok ürünü seçin.");
                ViewBag.Stocks = new SelectList(_stockService.GetStocksByHospital(user.HospitalID), "StockID", "Product.ProductName");
                return View();
            }

            return RedirectToAction("Edit", new { id = stockId });
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var stock = _context.Stocks
                                .Include(s => s.Product)
                                .Include(s => s.Hospital)
                                .FirstOrDefault(s => s.StockID == id);

            if (stock == null)
            {
                return NotFound();
            }

            return View(stock); // Silme onay sayfasını gösterir
        }

        [HttpPost]
        public IActionResult Delete(int id, string confirm = "yes")
        {
            Stock stock = null; // stock değişkenini try bloğu dışında tanımlıyoruz

            try
            {
                stock = _context.Stocks
                                .Include(s => s.Product)
                                .Include(s => s.Hospital)
                                .FirstOrDefault(s => s.StockID == id);

                if (stock == null)
                {
                    ModelState.AddModelError("", "Silinmek istenen stok bulunamadı.");
                    return RedirectToAction("Index");
                }

                _stockService.DeleteStock(id);
                return RedirectToAction("Index"); // Başarıyla silindikten sonra listeye döner
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                ModelState.AddModelError("", "Stok silinirken bir hata oluştu. Bu stok başka tablolarla ilişkili olabilir ve silinemiyor olabilir.");
                return View(stock); // Hata olduğunda aynı silme sayfasına geri döner
            }
        }

        [HttpGet]
        public async Task<IActionResult> BarcodeStockEntry()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var viewModel = new BarcodeStockTransactionViewModel
            {
                TransactionType = "Giriş"
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BarcodeStockEntry(BarcodeStockTransactionViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {

                var stock = _stockService.GetStockByReferenceNumber(viewModel.ReferenceNumber, user.HospitalID);

                if (stock != null)
                {
                    stock.Quantity += viewModel.Quantity;
                    stock.LotNumber = viewModel.LotNumber;
                    stock.ExpiryDate = viewModel.ExpiryDate;
                    stock.LastUpdated = DateTime.Now;
                    if (stock.FlaconCount == 0)
                    {
                        var product = _context.Products.FirstOrDefault(p => p.ProductID == stock.ProductID);
                        stock.FlaconCount = product?.FlaconCountPerBox ?? 0;
                    }
                    _context.SaveChanges();
                }
                else
                {
                    var product = _context.Products.FirstOrDefault(p => p.ReferenceNumber == viewModel.ReferenceNumber);
                    if (product == null)
                    {
                        ModelState.AddModelError("", "Ürün bulunamadı. Lütfen geçerli bir referans numarası girin.");
                        return View(viewModel);
                    }

                    var newStock = new Stock
                    {
                        ProductID = product.ProductID,
                        HospitalID = user.HospitalID,
                        Quantity = viewModel.Quantity,
                        LotNumber = viewModel.LotNumber,
                        ExpiryDate = viewModel.ExpiryDate,
                        LastUpdated = DateTime.Now,
                        FlaconCount = product.FlaconCountPerBox
                    };

                    _context.Stocks.Add(newStock);
                    _context.SaveChanges();
                }

                TempData["SuccessMessage"] = "Stok girişi başarılı bir şekilde yapıldı.";
                return RedirectToAction("BarcodeStockEntry");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                ModelState.AddModelError("", "Stok işlemi yapılırken bir hata oluştu. Lütfen tekrar deneyin.");
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> BarcodeStockExit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var viewModel = new BarcodeStockTransactionViewModel
            {
                TransactionType = "Çıkış"
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BarcodeStockExit(BarcodeStockTransactionViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                // Hastane ID'sine göre stok bulmak için `hospitalId` parametresini ekleyin
                var stock = _stockService.GetStockByReferenceNumber(viewModel.ReferenceNumber, user.HospitalID);

                if (stock == null || stock.HospitalID != user.HospitalID)
                {
                    ModelState.AddModelError("", "Stok bulunamadı veya bu stok üzerinde işlem yapma yetkiniz yok.");
                    return View(viewModel);
                }

                if (stock.Quantity < viewModel.Quantity)
                {
                    ModelState.AddModelError("", "Yetersiz stok miktarı. Çıkış yaparken stok miktarı 0'ın altına düşemez.");
                    return View(viewModel);
                }

                // Stok çıkış işlemi (expiryDate ve lotNumber dahil edildi)
                _stockService.ProcessStockTransaction(
                    viewModel.ReferenceNumber,
                    -viewModel.Quantity, // Çıkış işlemi için negatif miktar
                    user.HospitalID,
                    viewModel.LotNumber,
                    viewModel.ExpiryDate
                );

                TempData["SuccessMessage"] = "Stok çıkışı başarılı bir şekilde yapıldı.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                ModelState.AddModelError("", "Stok işlemi yapılırken bir hata oluştu. Lütfen tekrar deneyin.");
                return View(viewModel);
            }
        }

        // ====== Test bazlı barkod sayımı (Kutu/Test seçimli, toplayarak) ======

        [HttpGet]
        public async Task<IActionResult> BarcodeCount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Tek hesapla tüm hastanelere giriş: hastaneyi kullanıcı ekranda seçer.
            var hospitals = _context.Hospitals
                                    .OrderBy(h => h.HospitalName)
                                    .ToList();

            // Varsayılan seçim: kullanıcının kendi hastanesi (varsa), yoksa ilk hastane.
            int defaultHospitalId = hospitals.Any(h => h.HospitalID == user.HospitalID)
                ? user.HospitalID
                : (hospitals.FirstOrDefault()?.HospitalID ?? 0);

            ViewBag.Hospitals = new SelectList(hospitals, "HospitalID", "HospitalName", defaultHospitalId);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BarcodeCount(string itemsJson, int hospitalId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (string.IsNullOrWhiteSpace(itemsJson))
            {
                TempData["ErrorMessage"] = "Sayım listesi boş. En az bir satır ekleyin.";
                return RedirectToAction("BarcodeCount");
            }

            // Hastane seçimi zorunlu ve geçerli olmalı (tek hesap, çok hastane).
            if (hospitalId <= 0 || !_context.Hospitals.Any(h => h.HospitalID == hospitalId))
            {
                TempData["ErrorMessage"] = "Lütfen geçerli bir hastane seçin.";
                return RedirectToAction("BarcodeCount");
            }

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var lines = JsonSerializer.Deserialize<List<StockCountLineDTO>>(itemsJson, options)
                            ?? new List<StockCountLineDTO>();

                var result = _stockService.ApplyCount(lines, hospitalId, user.Id);

                if (result.LineCount == 0)
                {
                    TempData["ErrorMessage"] = "Hiçbir satır işlenemedi. " +
                        (result.Warnings.Count > 0 ? string.Join(" ", result.Warnings) : "");
                    return RedirectToAction("BarcodeCount");
                }

                var msg = $"Sayım kaydedildi: {result.ProductCount} ürün, {result.LineCount} satır, toplam {result.TotalTests} test.";
                if (result.Warnings.Count > 0)
                    msg += " Uyarılar: " + string.Join(" ", result.Warnings);

                TempData["SuccessMessage"] = msg;
                return RedirectToAction("BarcodeCount");
            }
            catch (Exception ex)
            {
                // Gerçek kök nedeni bul (DbUpdateException ise asıl mesaj InnerException'da olur)
                var root = ex;
                while (root.InnerException != null) root = root.InnerException;

                Console.WriteLine("SAYIM HATASI: " + ex.ToString()); // VS Output penceresinde tam stack
                TempData["ErrorMessage"] = "Hata: " + root.Message;   // ekranda asıl mesaj
                return RedirectToAction("BarcodeCount");
            }
        }

        // ====== Hastaneler arası aktarım ======

        [HttpGet]
        public async Task<IActionResult> Transfer()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var hospitals = _context.Hospitals.OrderBy(h => h.HospitalName).ToList();
            ViewBag.Hospitals = new SelectList(hospitals, "HospitalID", "HospitalName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(string referenceNumber, int quantity, string unit,
            int sourceHospitalId, int destinationHospitalId, string note)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var result = _stockService.TransferBetweenHospitals(
                referenceNumber, quantity, unit, sourceHospitalId, destinationHospitalId, note, user.Id);

            if (result.Success)
                TempData["SuccessMessage"] = result.Message;
            else
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction("Transfer");
        }

        // Aktarım ekranında kaynak stoğu göstermek için
        [HttpGet]
        public JsonResult GetStockInfo(string referenceNumber, int hospitalId)
        {
            if (string.IsNullOrWhiteSpace(referenceNumber) || hospitalId <= 0)
                return Json(new { success = false });

            int tests = _stockService.GetTestCount(referenceNumber, hospitalId);
            return Json(new { success = true, testCount = tests });
        }

        // Referans numarasıyla doğrudan ürün bulma (barkod okunamadığında elle giriş için)
        [HttpGet]
        public JsonResult GetProductByReference(string referenceNumber)
        {
            if (string.IsNullOrWhiteSpace(referenceNumber))
            {
                return Json(new { success = false, message = "Referans numarası boş." });
            }

            var refNo = referenceNumber.Trim();
            var product = _context.Products
                .Where(p => p.ReferenceNumber == refNo)
                .Select(p => new
                {
                    p.ReferenceNumber,
                    p.ProductName,
                    p.BoxTextCount,
                    p.FlaconCountPerBox
                })
                .FirstOrDefault();

            if (product == null)
            {
                return Json(new { success = false, message = $"'{refNo}' referanslı ürün bulunamadı." });
            }

            return Json(new
            {
                success = true,
                product = new
                {
                    referenceNumber = product.ReferenceNumber,
                    productName = product.ProductName,
                    boxTextCount = product.BoxTextCount,
                    flaconCountPerBox = product.FlaconCountPerBox,
                    lotNumber = "",
                    expiryDate = "",
                    expiryIso = ""
                }
            });
        }

        // ====== Sayım Dönemi ======

        // Seçili hastanenin aktif sayım bilgisini döndürür (ekranda göstermek için)
        [HttpGet]
        public JsonResult GetActiveSayimInfo(int hospitalId)
        {
            if (hospitalId <= 0) return Json(new { success = false });
            var sayim = _stockService.GetActiveSayim(hospitalId);
            if (sayim == null)
                return Json(new { success = true, exists = false });

            return Json(new
            {
                success = true,
                exists = true,
                sayimNo = sayim.SayimNo,
                createdDate = sayim.CreatedDate.ToString("dd.MM.yyyy HH:mm")
            });
        }

        // Seçili hastane için yeni sayım başlatır (aktifi kapatır, No+1 yeni aktif açar)
        [HttpPost]
        public async Task<JsonResult> StartNewSayim(int hospitalId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "Oturum bulunamadı." });
            if (hospitalId <= 0 || !_context.Hospitals.Any(h => h.HospitalID == hospitalId))
                return Json(new { success = false, message = "Geçerli bir hastane seçin." });

            var sayim = _stockService.StartNewSayim(hospitalId, user.Id);
            return Json(new
            {
                success = true,
                sayimNo = sayim.SayimNo,
                createdDate = sayim.CreatedDate.ToString("dd.MM.yyyy HH:mm")
            });
        }

        [HttpGet]
        public JsonResult GetProductDetailsByBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return Json(new { success = false, message = "Barkod boş." });
            }

            BarcodeData barcodeData;
            try
            {
                barcodeData = BarcodeParser.Parse(barcode);
            }
            catch
            {
                return Json(new { success = false, message = "Barkod okunamadı. Lütfen referansı elle girin." });
            }

            if (string.IsNullOrEmpty(barcodeData.ReferenceNumber))
            {
                return Json(new { success = false, message = "Barkoddan referans numarası okunamadı. Lütfen referansı elle girin." });
            }

            var product = _context.Products
                .Where(p => p.ReferenceNumber == barcodeData.ReferenceNumber)
                .Select(p => new
                {
                    ProductName = p.ProductName,
                    BoxTextCount = p.BoxTextCount,
                    FlaconCountPerBox = p.FlaconCountPerBox
                })
                .FirstOrDefault();

            if (product == null)
            {
                return Json(new { success = false, message = "Ürün bulunamadı." });
            }

            // Ürün bilgisi başarıyla bulundu; JSON ile döndür.
            return Json(new
            {
                success = true,
                product = new
                {
                    referenceNumber = barcodeData.ReferenceNumber,
                    productName = product.ProductName,
                    boxTextCount = product.BoxTextCount,                 // 1 kutu = kaç test
                    flaconCountPerBox = product.FlaconCountPerBox,
                    lotNumber = barcodeData.LotNumber, // Barkoddan gelen Lot Number
                    expiryDate = barcodeData.ExpiryDate?.ToString("dd.MM.yyyy") ?? "Bilinmiyor", // görsel
                    expiryIso = barcodeData.ExpiryDate?.ToString("yyyy-MM-dd") ?? "" // date input için
                }
            });
        }

        [HttpGet]
        public JsonResult GetProductByReferenceNumber(string referenceNumber)
        {
            var product = _context.Products
                            .Where(p => p.ReferenceNumber == referenceNumber)
                            .Select(p => new
                            {
                                ProductID = p.ProductID,
                                ReferenceNumber = p.ReferenceNumber,
                                ProductName = p.ProductName
                            })
                            .FirstOrDefault();

            if (product == null)
            {
                return Json(new { success = false, message = "Ürün bulunamadı." });
            }

            return Json(new
            {
                success = true,
                product = new
                {
                    productId = product.ProductID,
                    referenceNumber = product.ReferenceNumber,
                    productName = product.ProductName
                }
            });
        }

        [HttpGet]
        public IActionResult ManageFlacon(int id)
        {
            var stock = _stockService.GetStockById(id);

            if (stock == null)
            {
                TempData["ErrorMessage"] = "Seçilen stok bulunamadı.";
                return RedirectToAction("Index");
            }

            if (stock.Product == null)
            {
                TempData["ErrorMessage"] = "Stok ilişkili ürün bulunamadı.";
                return RedirectToAction("Index");
            }

            if (stock.Hospital == null)
            {
                TempData["ErrorMessage"] = "Stok ilişkili hastane bulunamadı.";
                return RedirectToAction("Index");
            }

            // Product tablosundan flakon sayısını alıp Stock tablosundaki değeri eşitle
            if (stock.FlaconCount == 0)
            {
                stock.FlaconCount = stock.Product.FlaconCountPerBox;
                _stockService.UpdateStock(stock); // Güncelleme işlemi
            }

            var viewModel = new FlaconManagementViewModel
            {
                StockID = stock.StockID,
                ProductName = stock.Product.ProductName,
                CurrentFlaconCount = stock.FlaconCount, // Stok tablosundaki flakon sayısı
                HospitalName = stock.Hospital.HospitalName
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ManageFlacon(FlaconManagementViewModel model)
        {
            try
            {
                var stock = _stockService.GetStockById(model.StockID);
                if (stock == null)
                {
                    TempData["ErrorMessage"] = "Stok bulunamadı.";
                    return RedirectToAction("Index");
                }

                if (model.ChangeAmount <= 0)
                {
                    TempData["ErrorMessage"] = "Geçerli bir flakon miktarı giriniz.";
                    return RedirectToAction("ManageFlacon", new { id = model.StockID });
                }

                if (stock.FlaconCount < model.ChangeAmount)
                {
                    TempData["ErrorMessage"] = "Yeterli flakon yok.";
                    return RedirectToAction("ManageFlacon", new { id = model.StockID });
                }

                stock.FlaconCount -= model.ChangeAmount;
                _stockService.UpdateStock(stock);


                TempData["SuccessMessage"] = $"Flakon sayısı başarıyla güncellendi. Kalan flakon: {stock.FlaconCount}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Flakon değişim işlemi sırasında bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }


        [HttpGet("/Stock/ApiList")]
        [AllowAnonymous]
        public async Task<IActionResult> ApiList(
            [FromQuery] int? hospitalId,
            [FromQuery] int? sayimNo,
            [FromQuery] string? apiKey,
            [FromHeader(Name = "X-Api-Key")] string? headerKey,
            CancellationToken ct)
        {
            // ─── API KEY DOĞRULAMA ───
            var expected = _config["StockApi:ApiKey"];
            var provided = headerKey ?? apiKey;
            if (string.IsNullOrEmpty(expected) || string.IsNullOrEmpty(provided) || provided != expected)
                return Unauthorized(new { success = false, message = "Invalid or missing API key." });

            // ─── Query ───
            var q = _context.Stocks
                .Include(s => s.Product).ThenInclude(p => p.ProductType)
                .Include(s => s.Product).ThenInclude(p => p.Platform)
                .Include(s => s.Hospital)
                .Include(s => s.Sayim)
                .AsNoTracking()
                .AsQueryable();

            if (hospitalId.HasValue && hospitalId.Value > 0)
                q = q.Where(s => s.HospitalID == hospitalId.Value);

            // Sayım filtresi: belirtilen sayım, yoksa aktif
            if (sayimNo.HasValue && sayimNo.Value > 0)
                q = q.Where(s => s.Sayim.SayimNo == sayimNo.Value);
            else
                q = q.Where(s => s.Sayim.IsActive);

            var list = await q.Select(s => new StockApiItemDto
            {
                StockId = s.StockID,
                HospitalId = s.HospitalID,
                HospitalName = s.Hospital.HospitalName,
                ProductId = s.ProductID,
                ProductName = s.Product.ProductName,
                ReferenceNumber = s.Product.ReferenceNumber,
                ProductType = s.Product.ProductType != null ? s.Product.ProductType.TypeName : null,
                Platform = s.Product.Platform != null ? s.Product.Platform.PlatformName : null,
                BoxCount = s.Quantity,
                FlaconCount = s.FlaconCount,
                TestCount = s.TestCount,
                BoxTextCount = s.Product.BoxTextCount,
                FlaconCountPerBox = s.Product.FlaconCountPerBox,
                LotNumber = s.LotNumber,
                ExpiryDate = s.ExpiryDate,
                LastUpdated = s.LastUpdated,
                SayimNo = s.Sayim != null ? s.Sayim.SayimNo : 0,
                SayimAktifMi = s.Sayim != null && s.Sayim.IsActive,
                SayimTarihi = s.Sayim != null ? s.Sayim.CreatedDate : DateTime.MinValue
            }).ToListAsync(ct);

            return Ok(new StockApiResponseDto
            {
                Success = true,
                Count = list.Count,
                GeneratedAt = DateTime.Now,
                Items = list
            });
        }
    }
}