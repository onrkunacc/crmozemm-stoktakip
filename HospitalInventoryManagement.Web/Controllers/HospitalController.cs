using HospitalInventoryManagement.Data.Context;
using HospitalInventoryManagement.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HospitalInventoryManagement.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HospitalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Liste + ekleme ekranı
        public IActionResult Index()
        {
            var hospitals = _context.Hospitals
                .OrderBy(h => h.HospitalName)
                .ToList();

            // Her hastanedeki stok kaydı sayısı (silme kontrolü / bilgi için)
            ViewBag.StockCounts = _context.Stocks
                .GroupBy(s => s.HospitalID)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionary(x => x.Key, x => x.Count);

            return View(hospitals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string hospitalName)
        {
            if (string.IsNullOrWhiteSpace(hospitalName))
            {
                TempData["ErrorMessage"] = "Hastane adı boş olamaz.";
                return RedirectToAction("Index");
            }

            var name = hospitalName.Trim();
            if (_context.Hospitals.Any(h => h.HospitalName == name))
            {
                TempData["ErrorMessage"] = $"'{name}' zaten kayıtlı.";
                return RedirectToAction("Index");
            }

            _context.Hospitals.Add(new Hospital { HospitalName = name });
            _context.SaveChanges();
            TempData["SuccessMessage"] = $"'{name}' eklendi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var hospital = _context.Hospitals.Find(id);
            if (hospital == null)
            {
                TempData["ErrorMessage"] = "Hastane bulunamadı.";
                return RedirectToAction("Index");
            }
            return View(hospital);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string hospitalName)
        {
            var hospital = _context.Hospitals.Find(id);
            if (hospital == null)
            {
                TempData["ErrorMessage"] = "Hastane bulunamadı.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(hospitalName))
            {
                TempData["ErrorMessage"] = "Hastane adı boş olamaz.";
                return RedirectToAction("Edit", new { id });
            }

            hospital.HospitalName = hospitalName.Trim();
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Hastane güncellendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var hospital = _context.Hospitals.Find(id);
            if (hospital == null)
            {
                TempData["ErrorMessage"] = "Hastane bulunamadı.";
                return RedirectToAction("Index");
            }

            // Stok kaydı olan hastane silinmesin (yetim kayıt olmasın)
            if (_context.Stocks.Any(s => s.HospitalID == id))
            {
                TempData["ErrorMessage"] = "Bu hastaneye ait stok kayıtları var; önce onları taşıyın veya silin.";
                return RedirectToAction("Index");
            }

            _context.Hospitals.Remove(hospital);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Hastane silindi.";
            return RedirectToAction("Index");
        }
    }
}