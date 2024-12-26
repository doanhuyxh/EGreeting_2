using EGreeting.Data;
using EGreeting.Models;
using EGreeting.Models.RechargeViewModels;
using EGreeting.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EGreeting.Controllers
{
    public class RechargeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly UserManager<ApplicationUser> _userManager;

        public RechargeController(ApplicationDbContext context, ICommon iCommon, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _iCommon = iCommon;
            _userManager = userManager;


        }

        [HttpGet("nap-tien/{id}/{price}")]
        public IActionResult Index(int id, int price)
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
                ViewBag.CodeOrder = _iCommon.GenerateRandomString(6);
                ViewBag.PackageID = id;
                ViewBag.Price = price;
                return View();
            }
            return Redirect("/dang-nhap");


        }

        [HttpPost("confirm/nap-tien")]
        public async Task<IActionResult> ConfirmRecharge(RechargeCRUD model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);


                if (user == null)
                {
                    return Redirect("/dang-nhap");

                }
                var napTien = new Recharge
                {
                    UserID = user.Id,
                    PackageID = model.PackageID,
                    CodeRecharge = model.OrderCode,
                    PriceRecharge = model.PriceRecharge,
                    Status = false,
                    CreateDate = DateTime.Now,
                };
                user.TotalMoney += model.PriceRecharge;
                await _userManager.UpdateAsync(user);
                await _context.AddAsync(napTien);
                await _context.SaveChangesAsync();
                return Json(new { code = 200, message = "Payment confirmed" });

            }
            return Redirect("/dang-nhap");
        }
    }
}
