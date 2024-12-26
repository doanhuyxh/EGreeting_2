using EGreeting.Data;
using EGreeting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Areas.AdminGreenting.Controllers
{
    [Area("AdminGreenting")]
    [Authorize(Roles = "Admin")]
    public class AdminPackageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AdminPackageController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpGet("admin/list-package")]
        public async Task<IActionResult> Index()
        {
            var package = await _context.Packages.OrderByDescending(x => x.CreateDate).ToListAsync();
            return View(package);
        }
        [HttpPost("admin/add-package")]
        public async Task<IActionResult> AddPackage(Packages model)
        {
            if (model.Price == 0)
            {
                return Json(new { code = 400, message = "The package price must be greater than 0" });
            }
            if (string.IsNullOrEmpty(model.Decription))
            {
                return Json(new { code = 400, message = "Description cannot be empty" });
            }
            var checkPackage = await _context.Packages.Where(x => x.Duration.Contains(model.Duration)).FirstOrDefaultAsync();
            if (checkPackage != null)
            {
                return Json(new { code = 400, message = "Package already exists" });

            }
            try
            {
                model.CreateDate = DateTime.Now;
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Added successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
        [HttpGet("admin/get-package/{id}")]
        public async Task<IActionResult> GetPackage(int id)
        {
            var package = await _context.Packages.FirstOrDefaultAsync(x => x.IDPackage == id);
            if (package != null)
            {
                return Json(new { code = 200, data = package });

            }
            return Json(new { code = 400, data = package, message = "Package not found" });

        }
        [HttpPost("admin/edit-package")]
        public async Task<IActionResult> EditPackage(Packages model)
        {
            var exitPackage = await _context.Packages.FirstOrDefaultAsync(x => x.IDPackage == model.IDPackage);
            if (exitPackage == null)
            {
                return Json(new { code = 400, message = "Package not found" });

            }
            if (model.Price == 0)
            {
                return Json(new { code = 400, message = "The package price must be greater than 0" });

            }
            if (string.IsNullOrEmpty(model.Decription))
            {
                return Json(new { code = 400, message = "Description cannot be empty" });

            }
            var checkPackage = await _context.Packages.Where(x => x.Duration.Contains(model.Duration) && x.IDPackage != model.IDPackage).FirstOrDefaultAsync();
            if (checkPackage != null)
            {
                return Json(new { code = 400, message = "Package already exists" });

            }
            try
            {
                exitPackage.Price = model.Price;
                exitPackage.Decription = model.Decription;
                exitPackage.Duration = model.Duration;
                exitPackage.CreateDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Package repair successful" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
        [HttpPost("admin/remove-package")]
        public async Task<IActionResult> RemovePackage(int id)
        {

            try
            {
                var package = await _context.Packages.FirstOrDefaultAsync(x => x.IDPackage == id);
                if (package == null)
                {
                    return Json(new { code = 400, message = "Package not found" });

                }

                var listOrder = await _context.OrdersPackage.Where(x => x.PackageID == package.IDPackage).ToListAsync();
                if (listOrder != null && listOrder.Count > 0)
                {
                    _context.RemoveRange(listOrder);
                }
                _context.Remove(package);
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Package deletion successful" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
    }
}
