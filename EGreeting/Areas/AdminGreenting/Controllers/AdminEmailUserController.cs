using EGreeting.Data;
using EGreeting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Areas.AdminGreenting.Controllers
{
    [Area("AdminGreenting")]
    [Authorize(Roles = "Admin")]
    public class AdminEmailUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AdminEmailUserController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }
        [HttpGet("admin/email")]
        public async Task<IActionResult> Index()
        {
            var email = await _context.EmailOrders.Include(x => x.User).Include(x => x.EmailLists).Include(x => x.Card).ToListAsync();
            return View(email);
        }

        [HttpPost("admin/detail-list-email")]
        public async Task<IActionResult> GetListEmailDetail(int id)
        {
            var email = await _context.EmailOrders.FirstOrDefaultAsync(x => x.IDEmailOrder == id);
            if (email == null)
            {
                return Json(new { code = 400, message = "No listing found" });
            }

            var listEmail = await _context.EmailList.Where(x => x.EmailOrderID == email.IDEmailOrder).ToListAsync();
            return Json(new { code = 200, message = "Success list", data = listEmail });

        }
        [HttpPost("admin/remove-email")]
        public async Task<IActionResult> RemoveEmail(int id)
        {

            try
            {
                var email = await _context.EmailOrders.FirstOrDefaultAsync(x => x.IDEmailOrder == id);

                if (email == null)
                {
                    return Json(new { code = 400, message = "Email not found" });

                }

                var listEmail = await _context.EmailList.Where(x => x.EmailOrderID == email.IDEmailOrder).ToListAsync();
                if (listEmail != null && listEmail.Count > 0)
                {
                    _context.RemoveRange(listEmail);
                }
                _context.Remove(email);
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Email deleted successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
    }
}
