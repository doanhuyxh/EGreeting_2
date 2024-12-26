using EGreeting.Data;
using EGreeting.Models;
using EGreeting.Models.CardViewModels;
using EGreeting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Areas.AdminGreenting.Controllers
{
    [Area("AdminGreenting")]
    [Authorize(Roles = "Admin")]
    public class AdminCardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public AdminCardController(ApplicationDbContext context, ICommon iCommon, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _iCommon = iCommon;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("admin/card")]
        public async Task<IActionResult> Index()
        {
            var cards = await _context.Cards.Include(x => x.Categories).ToListAsync();
            return View(cards);
        }

        [HttpGet("admin/font-list")]
        public IActionResult FontList()
        {
            string physicalPath = Path.Combine(_hostingEnvironment.WebRootPath, "assets", "Font-chu-dep");

            if (!Directory.Exists(physicalPath))
            {
                return NotFound("Font directory does not exist.");
            }

            string[] files = Directory.GetFiles(physicalPath);

            Dictionary<string, string> fontlist = new Dictionary<string, string>();

            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string virtualPath = $"/assets/Font-chu-dep/{Path.GetFileName(file)}";
                fontlist[fileName] = virtualPath;
            }


            return Ok(fontlist);
        }

        [HttpGet("admin/form-card")]
        public async Task<IActionResult> FormCard([FromQuery] int id)
        {
            CardViewModelsCRUD model = new CardViewModelsCRUD();

            if (id != 0)
            {
                var card = await _context.Cards.Include(x => x.Categories).FirstOrDefaultAsync(x => x.IDCard == id);
                if (card != null)
                {
                    model = card;
                }
            }
            return View(model);
        }


        [HttpPost("admin/save-card")]
        public async Task<IActionResult> AddCard(CardViewModelsCRUD model)
        {

            if (string.IsNullOrEmpty(model.NameCard))
            {
                return Json(new { code = 400, message = "Tên thẻ không để trống" });

            }
            if (model.CategoryID == 0)
            {
                return Json(new { code = 400, message = "Danh mục thẻ không để trống" });

            }
            try
            {
                string slug = model.NameCard.ToSlug();
                model.Slug = slug;
                if (model.ImageCardPreviewPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.ImageCardPreviewPath);
                    model.ImageCardPreview = "/Upload/" + PrPath;
                    model.ImageCardMain = "/Upload/" + PrPath;
                }               

                Cards card = new Cards();

                if (model.IDCard != 0)
                {
                    card = await _context.Cards.FirstOrDefaultAsync(x => x.IDCard == model.IDCard);

                    if (card == null)
                    {
                        return Json(new { code = 400, message = "Không tìm thấy thẻ" });

                    }

                    if (model.ImageCardPreviewPath == null)
                    {
                        model.ImageCardPreview = card.ImageCardPreview;
                        model.ImageCardMain = card.ImageCardMain;
                    }

                    model.CreateDate = card.CreateDate;
                    _context.Entry(card).CurrentValues.SetValues(model);
                }
                else
                {
                    model.CreateDate = DateTime.Now;
                    Cards cardEntity = model;
                    await _context.Cards.AddAsync(cardEntity);
                }

                await _context.SaveChangesAsync();


                return Json(new { code = 200, message = "Thêm mới thành công" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
        [HttpGet("admin/get-card/{id}")]
        public async Task<IActionResult> GetCard(int id)
        {
            var card = await _context.Cards.Include(x => x.Categories).FirstOrDefaultAsync(x => x.IDCard == id);
            if (card != null)
            {
                return Json(new { code = 200, data = card });

            }
            return Json(new { code = 400, data = card, message = "Không tìm thấy thẻ" });

        }


        [HttpPost("admin/remove-card")]
        public async Task<IActionResult> RemoveCard(int id)
        {

            try
            {
                var exitCard = await _context.Cards.FirstOrDefaultAsync(x => x.IDCard == id);

                if (exitCard == null)
                {
                    return Json(new { code = 400, message = "Không tìm thấy thẻ" });

                }

                _context.Remove(exitCard);
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Xóa thẻ thành công" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message });

            }

        }
    }
}
