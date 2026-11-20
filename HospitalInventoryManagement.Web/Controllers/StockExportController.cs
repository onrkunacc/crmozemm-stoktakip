using System;
using System.IO;
using System.Linq;
using HospitalInventoryManagement.Data.Context;
using HospitalInventoryManagement.Data.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace HospitalInventoryManagement.Web.Controllers
{
    [Authorize]
    public class StockExportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockExportController(ApplicationDbContext context)
        {
            _context = context;
        }

        private (System.Collections.Generic.List<Stock> stocks, string scopeName) GetFiltered(string searchQuery, int? hospitalId, int? sayimNo)
        {
            var query = _context.Stocks
                .Include(s => s.Product).ThenInclude(p => p.Category)
                .Include(s => s.Hospital)
                .Include(s => s.Sayim)
                .AsQueryable();

            if (hospitalId.HasValue && hospitalId.Value > 0)
                query = query.Where(s => s.HospitalID == hospitalId.Value);

            if (!string.IsNullOrWhiteSpace(searchQuery))
                query = query.Where(s => s.Product.ProductName.Contains(searchQuery));

            // Sayım filtresi: seçilen sayım numarası; yoksa güncel (aktif) sayım.
            // Stok Listesi ekranı ile birebir aynı davranış.
            if (sayimNo.HasValue && sayimNo.Value > 0)
                query = query.Where(s => s.Sayim.SayimNo == sayimNo.Value);
            else
                query = query.Where(s => s.Sayim.IsActive);

            var stocks = query
                .OrderBy(s => s.Hospital.HospitalName)
                .ThenBy(s => s.Product.ProductName)
                .ToList();

            string scope = (hospitalId.HasValue && hospitalId.Value > 0)
                ? (_context.Hospitals.FirstOrDefault(h => h.HospitalID == hospitalId.Value)?.HospitalName ?? "Hastane")
                : "Tüm Hastaneler";

            scope += (sayimNo.HasValue && sayimNo.Value > 0)
                ? $" · Sayım #{sayimNo.Value}"
                : " · Güncel Sayım";

            return (stocks, scope);
        }

        private static int Boxes(Stock s) => (s.Product != null && s.Product.BoxTextCount > 0) ? s.TestCount / s.Product.BoxTextCount : 0;
        private static int Rem(Stock s) => (s.Product != null && s.Product.BoxTextCount > 0) ? s.TestCount % s.Product.BoxTextCount : s.TestCount;
        private static string BoxTest(Stock s) => Boxes(s) > 0 ? $"{Boxes(s)} kutu + {Rem(s)} test" : $"{s.TestCount} test";

        // 0=uygun, 1=yaklaşıyor, 2=geçti
        private static int Status(Stock s, DateTime now, DateTime soon)
            => s.ExpiryDate.Date < now.Date ? 2 : (s.ExpiryDate <= soon ? 1 : 0);
        private static string StatusText(Stock s, DateTime now, DateTime soon)
        {
            int st = Status(s, now, soon);
            if (st == 2) return "Miadı Geçti";
            if (st == 1) return $"Yaklaşıyor ({(s.ExpiryDate.Date - now.Date).Days} gün)";
            return "Uygun";
        }

        private static string SafeFile(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars()) name = name.Replace(c, '_');
            return name.Replace(' ', '_');
        }

        // ================= EXCEL =================
        [HttpGet]
        public IActionResult Excel(string searchQuery, int? hospitalId, int? sayimNo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var now = DateTime.Now; var soon = now.AddDays(60);
            var (stocks, scope) = GetFiltered(searchQuery, hospitalId, sayimNo);

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Stok");

            ws.Cells[1, 1].Value = "ÖZEM - Stok Listesi";
            ws.Cells[1, 1, 1, 10].Merge = true;
            ws.Cells[1, 1].Style.Font.Bold = true;
            ws.Cells[1, 1].Style.Font.Size = 14;

            ws.Cells[2, 1].Value = $"Kapsam: {scope}";
            ws.Cells[2, 1, 2, 5].Merge = true;
            ws.Cells[2, 6].Value = $"Tarih: {DateTime.Now:dd.MM.yyyy HH:mm}";
            ws.Cells[2, 6, 2, 10].Merge = true;
            ws.Cells[2, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            int headRow = 4;
            string[] headers = { "Ürün Adı", "Referans", "Lot", "Hastane", "Kategori", "Test", "Kutu / Test", "Flakon", "SKT", "Durum" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cells[headRow, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(52, 73, 94));
                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }

            int row = headRow + 1;
            int totalTests = 0;
            foreach (var s in stocks)
            {
                int st = Status(s, now, soon);
                ws.Cells[row, 1].Value = s.Product?.ProductName;
                ws.Cells[row, 2].Value = s.Product?.ReferenceNumber;
                ws.Cells[row, 3].Value = s.LotNumber;
                ws.Cells[row, 4].Value = s.Hospital?.HospitalName;
                ws.Cells[row, 5].Value = s.Product?.Category?.CategoryName;
                ws.Cells[row, 6].Value = s.TestCount;
                ws.Cells[row, 7].Value = BoxTest(s);
                ws.Cells[row, 8].Value = s.FlaconCount;
                ws.Cells[row, 9].Value = s.ExpiryDate.ToString("dd.MM.yyyy");
                ws.Cells[row, 10].Value = StatusText(s, now, soon);

                // Miat durumuna göre SKT + Durum hücresini renklendir
                if (st == 2)
                {
                    foreach (int c in new[] { 9, 10 })
                    {
                        ws.Cells[row, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(248, 215, 218));
                        ws.Cells[row, c].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(148, 41, 41));
                        ws.Cells[row, c].Style.Font.Bold = true;
                    }
                }
                else if (st == 1)
                {
                    foreach (int c in new[] { 9, 10 })
                    {
                        ws.Cells[row, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 243, 205));
                        ws.Cells[row, c].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(133, 100, 4));
                    }
                }

                totalTests += s.TestCount;
                row++;
            }

            ws.Cells[row, 1].Value = $"Toplam kayıt: {stocks.Count}";
            ws.Cells[row, 1, row, 5].Merge = true;
            ws.Cells[row, 5].Value = "Toplam Test:";
            ws.Cells[row, 6].Value = totalTests;
            ws.Cells[row, 1, row, 10].Style.Font.Bold = true;

            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            ws.View.FreezePanes(headRow + 1, 1);

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            string fileName = $"Stok_Listesi_{SafeFile(scope)}_{DateTime.Now:yyyyMMdd}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // ================= PDF =================
        [HttpGet]
        public IActionResult Pdf(string searchQuery, int? hospitalId, int? sayimNo)
        {
            var now = DateTime.Now; var soon = now.AddDays(60);
            var (stocks, scope) = GetFiltered(searchQuery, hospitalId, sayimNo);

            var stream = new MemoryStream();
            var document = new Document(PageSize.A4.Rotate(), 24, 24, 28, 28);
            var writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;

            try
            {
                Font titleFont, headFont, cellFont, totalFont;
                BaseFont baseF;
                var white = new BaseColor(255, 255, 255);
                try
                {
                    var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    baseF = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                catch
                {
                    baseF = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                }
                titleFont = new Font(baseF, 15, Font.BOLD);
                headFont = new Font(baseF, 9, Font.BOLD, white);
                cellFont = new Font(baseF, 8, Font.NORMAL);
                totalFont = new Font(baseF, 10, Font.BOLD);

                document.Open();
                document.Add(new Paragraph("ÖZEM - Stok Listesi", titleFont) { Alignment = Element.ALIGN_CENTER });
                document.Add(new Paragraph($"Kapsam: {scope}", cellFont) { Alignment = Element.ALIGN_CENTER });
                document.Add(new Paragraph($"Tarih: {DateTime.Now:dd.MM.yyyy HH:mm}", cellFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 6 });
                // Açıklama
                document.Add(new Paragraph("Kırmızı satır: miadı geçti · Turuncu: 60 gün içinde", cellFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 10 });

                var headerBg = new BaseColor(52, 73, 94);
                var zebra = new BaseColor(247, 249, 250);
                var expiredBg = new BaseColor(248, 215, 218);
                var expiredText = new BaseColor(148, 41, 41);
                var soonBg = new BaseColor(255, 243, 205);
                var soonText = new BaseColor(133, 100, 4);
                var okText = new BaseColor(39, 128, 96);

                var table = new PdfPTable(9) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 3.0f, 1.4f, 1.4f, 2.2f, 0.9f, 1.9f, 0.9f, 1.3f, 1.6f });
                table.HeaderRows = 1;

                string[] headers = { "Ürün Adı", "Referans", "Lot", "Hastane", "Test", "Kutu / Test", "Flakon", "SKT", "Durum" };
                foreach (var h in headers)
                {
                    table.AddCell(new PdfPCell(new Phrase(h, headFont))
                    {
                        BackgroundColor = headerBg,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Padding = 5
                    });
                }

                int totalTests = 0;
                bool alt = false;
                foreach (var s in stocks)
                {
                    int st = Status(s, now, soon);
                    var rowBg = st == 2 ? expiredBg : (alt ? zebra : white);
                    alt = !alt;

                    PdfPCell C(string text, int align = Element.ALIGN_LEFT, Font f = null) => new PdfPCell(new Phrase(text ?? "", f ?? cellFont))
                    {
                        BackgroundColor = rowBg,
                        HorizontalAlignment = align,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Padding = 4
                    };

                    // SKT ve Durum için renkli font (güvenli 4 parametreli constructor)
                    var durText = StatusText(s, now, soon);
                    Font sktFont, durFont;
                    if (st == 2) { sktFont = new Font(baseF, 8, Font.BOLD, expiredText); durFont = new Font(baseF, 8, Font.BOLD, expiredText); }
                    else if (st == 1) { sktFont = new Font(baseF, 8, Font.NORMAL, soonText); durFont = new Font(baseF, 8, Font.NORMAL, soonText); }
                    else { sktFont = cellFont; durFont = new Font(baseF, 8, Font.NORMAL, okText); }

                    table.AddCell(C(s.Product?.ProductName));
                    table.AddCell(C(s.Product?.ReferenceNumber));
                    table.AddCell(C(s.LotNumber));
                    table.AddCell(C(s.Hospital?.HospitalName));
                    table.AddCell(C(s.TestCount.ToString(), Element.ALIGN_RIGHT));
                    table.AddCell(C(BoxTest(s)));
                    table.AddCell(C(s.FlaconCount.ToString(), Element.ALIGN_RIGHT));
                    table.AddCell(C(s.ExpiryDate.ToString("dd.MM.yyyy"), Element.ALIGN_CENTER, sktFont));
                    table.AddCell(C(durText, Element.ALIGN_CENTER, durFont));

                    totalTests += s.TestCount;
                }

                document.Add(table);
                document.Add(new Paragraph($"Toplam kayıt: {stocks.Count}    |    Toplam Test: {totalTests}", totalFont)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingBefore = 10
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PDF oluşturulurken hata: {ex.Message}");
                return StatusCode(500, "PDF oluşturulurken bir hata oluştu.");
            }
            finally
            {
                document.Close();
                writer.Close();
            }

            stream.Position = 0;
            string fileName = $"Stok_Listesi_{SafeFile(scope)}_{DateTime.Now:yyyyMMdd}.pdf";
            return File(stream, "application/pdf", fileName);
        }
    }
}