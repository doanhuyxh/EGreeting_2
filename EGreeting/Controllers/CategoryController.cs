using EGreeting.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet("danh-muc/{slug}")]
        public async Task<IActionResult> Index(string slug)
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
            var category = await _context.CategoriesCard.FirstOrDefaultAsync(c => c.Slug == slug);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.category = category;

            var listProduct = await _context.Cards.Where(X => X.CategoryID == category.IDCategoryCard).ToListAsync();
            return View(listProduct);
        }
    }
}
