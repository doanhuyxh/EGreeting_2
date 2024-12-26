using EGreeting.Data;
using EGreeting.Models;
using EGreeting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Areas.AdminGreenting.Controllers
{
    [Area("AdminGreenting")]
    [Authorize(Roles = "Admin")]
    public class AdminRechargeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminRechargeController(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;

        }

        [HttpGet("admin/list-recharge")]
        public async Task<IActionResult> Index()
        {
            var recharges = await _context.Recharge.Include(x => x.UserData).Include(x => x.Package).OrderByDescending(x => x.CreateDate).ToListAsync();
            return View(recharges);
        }


        [HttpGet("admin/recharge/change-active/{id}")]
        public async Task<IActionResult> ChangeActive(int id)
        {
            try
            {
                var recharge = await _context.Recharge.Include(x => x.UserData).Include(x => x.Package).FirstOrDefaultAsync(X => X.IDRecharge == id);
                if (recharge == null)
                {
                    return Json(new { code = 400, message = "No deposit form found" });
                }
                var user = await _userManager.FindByIdAsync(recharge.UserData.Id);
                if (user == null)
                {
                    return Json(new { code = 400, message = "User not found" });
                }

                user.TotalMoney -= recharge.PriceRecharge;
                recharge.Status = true;
                DateTime endDate = recharge.Package.Duration.ToDate();
                var checkOrder = await _context.OrdersPackage.FirstOrDefaultAsync(x => x.UserID == user.Id && x.PackageID == recharge.PackageID);
                if (checkOrder != null)
                {
                    if (checkOrder.Status)
                    {
                        checkOrder.EndDate = endDate;

                    }
                    else
                    {
                        checkOrder.EndDate = endDate;
                        checkOrder.Status = true;

                    }
                }
                else
                {
                    var newOrder = new OrdersPackage
                    {
                        UserID = user.Id,
                        PackageID = recharge.PackageID,
                        CreateDate = DateTime.Now,
                        EndDate = endDate,
                        Status = true
                    };
                    await _context.OrdersPackage.AddAsync(newOrder);
                }

                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Success" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }
        }

        [HttpPost("admin/remove-recharge")]
        public async Task<IActionResult> RemoveRecharge(int id)
        {

            try
            {
                var recharge = await _context.Recharge.Include(x => x.UserData).FirstOrDefaultAsync(X => X.IDRecharge == id);

                if (recharge == null)
                {
                    return Json(new { code = 400, message = "No deposit method found" });

                }
                _context.Remove(recharge);
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Deleted deposit successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
    }
}
