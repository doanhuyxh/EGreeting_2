using EGreeting.Data;
using EGreeting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Areas.AdminGreenting.Controllers
{
    [Area("AdminGreenting")]
    [Authorize(Roles = "Admin")]
    public class AdminPackageOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminPackageOrdersController(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;

        }

        [HttpGet("admin/list-package-order")]
        public async Task<IActionResult> Index()
        {
            var package = await _context.OrdersPackage.Include(x => x.User).Include(x => x.Package).OrderByDescending(x => x.CreateDate).ToListAsync();
            return View(package);
        }




        [HttpPost("admin/remove-order-user")]
        public async Task<IActionResult> RemovePackageOrders(int id)
        {

            try
            {
                var package = await _context.OrdersPackage.FirstOrDefaultAsync(x => x.IDOrderPackage == id);
                if (package == null)
                {
                    return Json(new { code = 400, message = "No package purchases found" });

                }

                _context.Remove(package);
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Successfully deleted package purchase" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
    }
}
