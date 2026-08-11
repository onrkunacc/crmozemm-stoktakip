using HospitalInventoryManagement.BL.Interfaces;
using HospitalInventoryManagement.Data.Context;
using HospitalInventoryManagement.Data.DTOs;
using HospitalInventoryManagement.Data.Models;
using HospitalInventoryManagement.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using X.PagedList;

namespace HospitalInventoryManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStockService _stockService;
        private readonly IStatisticsService _statisticsService;
        private readonly ApplicationDbContext _context;

        public HomeController(UserManager<ApplicationUser> userManager, IStockService stockService, IStatisticsService statisticsService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _stockService = stockService;
            _statisticsService = statisticsService;
            _context = context;
        }

        // Ana Sayfa (test bazlý dashboard)
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Admin kullanýcýlar AdminIndex'e yönlendirilir
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("AdminIndex");
            }

            var stocks = _stockService.GetStocksByHospital(user.HospitalID);
            var now = DateTime.Now;
            var soon = now.AddDays(60);

            // Düþük stok = bir kutunun (BoxTextCount) altýna düþen test miktarý
            bool IsLow(Stock s) => s.Product != null && s.Product.BoxTextCount > 0 && s.TestCount < s.Product.BoxTextCount;

            var lowStockItems = stocks
                .Where(IsLow)
                .OrderBy(s => s.TestCount)
                .Take(10)
                .ToList();

            var stockData = stocks
                .Where(s => s.Product != null && s.TestCount > 0)
                .OrderByDescending(s => s.TestCount)
                .Take(12)
                .Select(s => new StockChartData
                {
                    ProductName = s.Product.ProductName,
                    Quantity = s.Quantity,
                    TestCount = s.TestCount
                }).ToList();

            var hospital = _context.Hospitals.FirstOrDefault(h => h.HospitalID == user.HospitalID);

            var viewModel = new HomeViewModel
            {
                HospitalName = hospital?.HospitalName ?? "-",
                LowStockItems = lowStockItems,
                StockData = stockData,
                TotalProducts = stocks.Count,
                TotalTests = stocks.Sum(s => s.TestCount),
                LowStockCount = stocks.Count(IsLow),
                ExpiringSoonCount = stocks.Count(s => s.ExpiryDate > now && s.ExpiryDate <= soon)
            };

            return View(viewModel);
        }

        // Admin Ana Sayfa
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AdminIndex(int? hospitalId, string sortOrder)
        {
            var viewModel = new AdminStockListViewModel
            {
                Hospitals = new SelectList(_context.Hospitals, "HospitalID", "HospitalName"),
                SelectedHospitalID = hospitalId,
                SortOrder = sortOrder
            };

            var stocks = _stockService.GetAllStocks();

            if (hospitalId.HasValue)
            {
                stocks = stocks.Where(s => s.HospitalID == hospitalId.Value).ToList();
            }

            switch (sortOrder)
            {
                case "NameAsc":
                    stocks = stocks.OrderBy(s => s.Product.ProductName).ToList();
                    break;
                case "NameDesc":
                    stocks = stocks.OrderByDescending(s => s.Product.ProductName).ToList();
                    break;
                case "DateAsc":
                    stocks = stocks.OrderBy(s => s.LastUpdated).ToList();
                    break;
                case "DateDesc":
                    stocks = stocks.OrderByDescending(s => s.LastUpdated).ToList();
                    break;
                default:
                    break;
            }

            var now = DateTime.Now;
            var soon = now.AddDays(60);

            viewModel.TotalRecords = stocks.Count;
            viewModel.TotalTests = stocks.Sum(s => s.TestCount);
            viewModel.LowStockCount = stocks.Count(s => s.Product != null && s.Product.BoxTextCount > 0 && s.TestCount < s.Product.BoxTextCount);
            viewModel.ExpiringSoonCount = stocks.Count(s => s.ExpiryDate > now && s.ExpiryDate <= soon);

            viewModel.ExpiringSoon = stocks
                .Where(s => s.ExpiryDate > now && s.ExpiryDate <= soon)
                .OrderBy(s => s.ExpiryDate)
                .Take(8)
                .Select(s => new StockDTO
                {
                    ProductName = s.Product.ProductName,
                    HospitalName = s.Hospital.HospitalName,
                    ExpiryDate = s.ExpiryDate,
                    TestCount = s.TestCount,
                    BoxTextCount = s.Product.BoxTextCount
                })
                .ToList();

            viewModel.Stocks = stocks.Select(s => new StockDTO
            {
                StockID = s.StockID,
                ProductName = s.Product.ProductName,
                Quantity = s.Quantity,
                TestCount = s.TestCount,
                BoxTextCount = s.Product.BoxTextCount,
                ExpiryDate = s.ExpiryDate,
                HospitalName = s.Hospital.HospitalName,
                LastUpdated = s.LastUpdated
            }).ToList();

            return View(viewModel);
        }
    }
}