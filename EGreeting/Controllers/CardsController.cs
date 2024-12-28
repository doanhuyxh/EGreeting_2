using EGreeting.Data;
using EGreeting.Models;
using EGreeting.Models.CardViewModels;
using EGreeting.Models.EmailListViewModels;
using EGreeting.Services;
using ImageProcessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public CardsController(ApplicationDbContext context, ICommon iCommon, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _iCommon = iCommon;
            _hostingEnvironment = hostingEnvironment;

        }

        [HttpGet("{slug}")]
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
            var card = await _context.Cards.Include(X => X.Categories).FirstOrDefaultAsync(c => c.Slug == slug);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        [HttpPost("user/send-email")]
        public async Task<IActionResult> SendEmailUser(EmailListCRUD model)
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


                var checkCard = await _context.Cards.FirstOrDefaultAsync(x => x.IDCard == model.CardID);
                if (checkCard == null)
                {
                    return Json(new { code = 400, message = "Card not found" });

                }
                var checkOrder = await _context.OrdersPackage.Where(x => x.UserID == user.Id && x.Status).ToListAsync();
                if (checkOrder.Count <= 0)
                {

                    return Json(new { code = 400, message = "Please purchase a package or renew before submitting" });

                }
                DateTime dateTimeNow = DateTime.Now;
                foreach (var userOrder in checkOrder)
                {
                    if (userOrder.EndDate.Date <= dateTimeNow.Date)
                    {
                        userOrder.Status = false;


                    }

                }
                await _iCommon.SendEmail(checkCard, model);

                var emailOrder = new EmailOrders
                {
                    UserID = user.Id,
                    CardID = checkCard.IDCard,
                    EmailSubject = model.EmailSubject,
                    CreateDate = DateTime.Now,
                };
                await _context.AddAsync(emailOrder);
                await _context.SaveChangesAsync();

                foreach (var order in model.EmailList)
                {
                    var emailOrderDetails = new EmailList
                    {
                        EmailOrderID = emailOrder.IDEmailOrder,
                        EmailSender = order,
                        Status = true,
                    };
                    await _context.AddAsync(emailOrderDetails);

                }
                await _context.SaveChangesAsync();

                return Json(new { code = 200, message = "Sent successfully" });
            }
            return Redirect("/dang-nhap");

        }

        [HttpPost]
        public IActionResult GenderText([FromBody] List<TextGenderViewModel> list_text, [FromQuery] int id_card)
        {

            Cards card = _context.Cards.FirstOrDefault(x => x.IDCard == id_card);

            if (card == null)
            {
                return Json(new { code = 400, message = "Card not found" });
            }

            List<TextOverlay> textOverlay = new List<TextOverlay>();
            foreach (var item in list_text)
            {
                textOverlay.Add(new TextOverlay
                {
                    X = item.x,
                    Y = item.y,
                    Text = item.text,
                    FontPath = _hostingEnvironment.ContentRootPath + "/wwwroot/" + item.font,
                    FontSize = item.font_size,
                    TextColor = item.color,
                    MaxWidth = item.max_width
                });
            }

            string path = _hostingEnvironment.ContentRootPath + "/wwwroot" + card.ImageCardMain;
            string relative_path = $"/images/{DateTime.Now.Ticks}.jpg";
            string path_out = _hostingEnvironment.ContentRootPath + $"/wwwroot/{relative_path}";
            ProcessingAddTextsToImage proset = new ProcessingAddTextsToImage
            {
                ImageSrcPath = path,
                ImageDestPath = path_out,
                Texts = textOverlay
            };

            ResultImageProcessing ImageProcessing = proset.AddTextsToImage();

            return Json(new { path = relative_path });
        }
    }
}
