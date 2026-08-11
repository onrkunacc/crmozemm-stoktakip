using HospitalInventoryManagement.BL.Interfaces;
using HospitalInventoryManagement.Data.Context;
using HospitalInventoryManagement.Data.Models;
using HospitalInventoryManagement.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalInventoryManagement.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        // Kullanıcı listesi (rolleriyle)
        public async Task<IActionResult> UserManagement()
        {
            var users = _userManager.Users.ToList();
            var userWithRoles = new List<UserWithRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userWithRoles.Add(new UserWithRolesViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return View(userWithRoles);
        }

        public IActionResult RoleManagement()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            return RedirectToAction("RoleManagement");
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _roleManager.RoleExistsAsync(roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
            return RedirectToAction("UserManagement");
        }

        // Grafik verisi (test bazlı, uyarlanır):
        //  - hospitalId yoksa: hastane bazında toplam test
        //  - hospitalId varsa: o hastanenin ürün bazında test stoğu (ilk 15)
        [HttpGet]
        public async Task<JsonResult> GetHospitalStocks(int? hospitalId)
        {
            if (hospitalId.HasValue && hospitalId.Value > 0)
            {
                var byProduct = await _context.Stocks
                    .Where(s => s.HospitalID == hospitalId.Value && s.TestCount > 0)
                    .OrderByDescending(s => s.TestCount)
                    .Take(15)
                    .Select(s => new { label = s.Product.ProductName, total = s.TestCount })
                    .ToListAsync();
                return Json(byProduct);
            }

            var byHospital = await _context.Stocks
                .GroupBy(s => s.Hospital.HospitalName)
                .Select(g => new { label = g.Key, total = g.Sum(s => s.TestCount) })
                .OrderByDescending(x => x.total)
                .ToListAsync();
            return Json(byHospital);
        }

        // Grafik / özet dashboard
        public IActionResult Index()
        {
            ViewBag.Hospitals = _context.Hospitals
                .Select(h => new { h.HospitalID, h.HospitalName })
                .ToList();

            var stocks = _context.Stocks.Include(s => s.Product).ToList();
            var now = DateTime.Now;
            var soon = now.AddDays(60);

            ViewBag.TotalHospitals = _context.Hospitals.Count();
            ViewBag.TotalProducts = _context.Products.Count();
            ViewBag.TotalTests = stocks.Sum(s => s.TestCount);
            ViewBag.LowStockCount = stocks.Count(s => s.Product != null && s.Product.BoxTextCount > 0 && s.TestCount < s.Product.BoxTextCount);
            ViewBag.ExpiringSoonCount = stocks.Count(s => s.ExpiryDate > now && s.ExpiryDate <= soon);

            return View();
        }

        public IActionResult UserList()
        {
            var users = _userManager.Users
                .Select(u => new AdminUserViewModel
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    HospitalName = u.HospitalID != null && u.HospitalID > 0
                        ? _context.Hospitals.Where(h => h.HospitalID == u.HospitalID).Select(h => h.HospitalName).FirstOrDefault()
                        : "Belirtilmemiş"
                })
                .ToList();

            return View(users);
        }
    }
}