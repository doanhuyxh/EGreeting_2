using EGreeting.Data;
using EGreeting.Models;
using Microsoft.AspNetCore.Mvc;

namespace EGreeting.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet("phan-hoi")]
        public IActionResult Index()
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

            return View();
        }

        [HttpPost("phan-hoi/post")]
        public async Task<IActionResult> FeedbackPost(Feedbacks model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400, message = "Please fill out all fields completely" });
            }

            model.CreateDate = DateTime.Now;
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return Json(new { code = 200, message = "We will contact you soon!!!" });

        }
    }
}
