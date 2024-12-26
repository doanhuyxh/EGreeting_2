using EGreeting.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGreeting.Areas.AdminGreenting.Controllers
{
    [Area("AdminGreenting")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AdminDashboardController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpGet("admin/dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
