using EGreeting.Data;
using EGreeting.Models;
using EGreeting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Areas.AdminGreenting.Controllers
{
    [Area("AdminGreenting")]
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AdminCategoryController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpGet("admin/category")]
        public async Task<IActionResult> Index()
        {
            var category = await _context.CategoriesCard.OrderByDescending(x => x.IDCategoryCard).ToListAsync();
            return View(category);
        }
        [HttpGet("admin/category-list")]
        public async Task<IActionResult> ListCategory()
        {
            var category = await _context.CategoriesCard.OrderByDescending(x => x.IDCategoryCard).ToListAsync();
            return Ok(category);
        }
        [HttpPost("admin/add-category")]
        public async Task<IActionResult> AddCategory(CategoriesCard model)
        {
            if (string.IsNullOrEmpty(model.NameCategory))
            {
                return Json(new { code = 400, message = "Category name cannot be empty" });
            }
            try
            {
                if (model.IsDefault)
                {
                    var category = await _context.CategoriesCard.FirstOrDefaultAsync(x => x.IsDefault);
                    if (category != null)
                    {
                        category.IsDefault = false;

                    }
                }
                string slug = model.NameCategory.ToSlug();
                model.Slug = slug;
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Added successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
        [HttpGet("admin/get-category/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.CategoriesCard.FirstOrDefaultAsync(x => x.IDCategoryCard == id);
            if (category != null)
            {
                return Json(new { code = 200, data = category });

            }
            return Json(new { code = 400, data = category, message = "No category found" });

        }
        [HttpPost("admin/edit-category")]
        public async Task<IActionResult> EditCategory(CategoriesCard model)
        {
            var exitCategory = await _context.CategoriesCard.FirstOrDefaultAsync(x => x.IDCategoryCard == model.IDCategoryCard);
            if (exitCategory == null)
            {
                return Json(new { code = 400, message = "No category found" });

            }
            if (string.IsNullOrEmpty(model.NameCategory))
            {
                return Json(new { code = 400, message = "Category name cannot be empty" });
            }
            try
            {

                if (model.IsDefault)
                {
                    var category = await _context.CategoriesCard.FirstOrDefaultAsync(x => x.IsDefault && x.IDCategoryCard != model.IDCategoryCard);
                    if (category != null)
                    {
                        category.IsDefault = false;

                    }


                }
                string slug = model.NameCategory.ToSlug();
                exitCategory.Slug = slug;
                exitCategory.NameCategory = model.NameCategory;
                exitCategory.IsComming = model.IsComming;
                exitCategory.IsOutstanding = model.IsOutstanding;
                exitCategory.ShortDecription = model.ShortDecription;
                if (exitCategory.IsDefault)
                {
                    exitCategory.IsDefault = true;
                }
                else
                {
                    exitCategory.IsDefault = model.IsDefault;

                }
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Edited directory successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
        [HttpPost("admin/remove-category")]
        public async Task<IActionResult> RemoveCategory(int id)
        {

            try
            {
                var category = await _context.CategoriesCard.FirstOrDefaultAsync(x => x.IDCategoryCard == id);
                if (category == null)
                {
                    return Json(new { code = 400, message = "No category found" });

                }
                if (category.IsDefault)
                {
                    return Json(new { code = 400, message = "Default categories cannot be deleted" });

                }
                var listCard = await _context.Cards.Where(x => x.CategoryID == category.IDCategoryCard).ToListAsync();
                if (listCard != null && listCard.Count > 0)
                {
                    var cateDf = await _context.CategoriesCard.FirstOrDefaultAsync(x => x.IsDefault);
                    if (cateDf != null)
                    {
                        foreach (var card in listCard)
                        {
                            {
                                card.CategoryID = cateDf.IDCategoryCard;
                            }
                        }
                    }
                }
                _context.Remove(category);
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Delete directory successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
    }
}
