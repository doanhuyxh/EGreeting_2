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
    public class AdminUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AdminUserController(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        [HttpGet("admin/list-user")]
        public async Task<IActionResult> Index()
        {
            var user = await _context.ApplicationUser.OrderByDescending(x => x.CreateDate).ToListAsync();
            return View(user);
        }

        [HttpGet("admin/user/change-active/{id}")]
        public async Task<IActionResult> ChangeActive(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return Json(new { code = 400, message = "User not found" });
                }
                user.IsActive = !user.IsActive;
                await _userManager.UpdateAsync(user);
                return Json(new { code = 200, message = "Success" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }
        }
        [HttpGet("admin/user/delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return Json(new { code = 400, message = "User not found" });

                }
                var listOrder = await _context.OrdersPackage.Where(x => x.UserID == user.Id).ToListAsync();
                if (listOrder.Count > 0)
                {
                    _context.RemoveRange(listOrder);
                }
                var listEmailOrder = await _context.EmailOrders.Where(x => x.UserID == user.Id).ToListAsync();
                if (listEmailOrder.Count > 0)
                {
                    _context.RemoveRange(listEmailOrder);

                }
                await _userManager.DeleteAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }
        }
        [HttpPost("admin/update-money")]
        public async Task<IActionResult> UpdateMoney(string id, decimal price)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return Json(new { code = 400, message = "User not found" });

                }

                user.TotalMoney = price;
                await _userManager.UpdateAsync(user);

                return Json(new { code = 200, message = "Added amount " + user.TotalMoney });



            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }
        }
    }
}
