using EGreeting.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Areas.AdminGreenting.Controllers
{
    [Area("AdminGreenting")]
    [Authorize(Roles = "Admin")]
    public class AdminFeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AdminFeedbackController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpGet("admin/feeback")]
        public async Task<IActionResult> Index()
        {
            var feedbaack = await _context.Feedbacks.ToListAsync();
            return View(feedbaack);
        }

        [HttpPost("admin/remove-feedback")]
        public async Task<IActionResult> RemoveFeedback(int id)
        {

            try
            {
                var feedbaack = await _context.Feedbacks.FirstOrDefaultAsync(X => X.IDFeedback == id);

                if (feedbaack == null)
                {
                    return Json(new { code = 400, message = "No feedback found" });

                }
                _context.Remove(feedbaack);
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Feedback deleted successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
    }
}
