using EGreeting.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Controllers
{
    public class PackageController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PackageController(ApplicationDbContext context)
        {
            _context = context;

        }
        [HttpGet("goi")]
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
                var listOrderPackage = await _context.OrdersPackage.Include(x => x.Package).Where(X => X.UserID == user.Id).ToListAsync();
                ViewBag.DataOrder = listOrderPackage;

            }
            var package = await _context.Packages.ToListAsync();

            return View(package);
        }
    }
}
