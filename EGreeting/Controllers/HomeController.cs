using EGreeting.Data;
using EGreeting.DTO;
using EGreeting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EGreeting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = (from u in _context.ApplicationUser
                            where u.UserName == User.Identity.Name
                            select new
                            {
                                u.Id,
                                u.FullName,
                                u.UserName,
                                u.TotalMoney,
                            }).FirstOrDefault();

                ViewBag.FullName = user.FullName;
                ViewBag.UserName = user.UserName;
                ViewBag.TotalMoney = user.TotalMoney;

            }
            var suKienDienRa = await _context.CategoriesCard.Where(x => x.IsComming).ToListAsync();
            var hangNgay = await _context.CategoriesCard.Where(x => x.IsOutstanding).ToListAsync();
            var defaultCate = await _context.CategoriesCard.Where(x => x.IsOutstanding == false && x.IsComming == false).ToListAsync();

            var response = new ViewHomeDTO
            {
                SuKienSapDienRa = suKienDienRa,
                HangNgay = hangNgay,
                ShowMacDinh = defaultCate,
            };

            return View(response);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
