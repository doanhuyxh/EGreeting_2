using EGreeting.Data;
using EGreeting.Models;
using EGreeting.Models.AccountVM;
using EGreeting.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EGreeting.Controllers
{
    public class AuthAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginVM> _logger;
        private readonly IConfiguration _iConfiguration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommon _icommon;
        public AuthAccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, ILogger<LoginVM> logger, IConfiguration iConfiguration, ICommon icommon)
        {
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
            _iConfiguration = iConfiguration;
            _userManager = userManager;
            _icommon = icommon;
        }

        [HttpGet("dang-nhap")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost("tai-khoan/dang-nhap")]
        public async Task<IActionResult> Index(LoginVM model)
        {
            model.UserName = model.UserName.ToLower();
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return new JsonResult(new
                {
                    code = 400,
                    status = "Error",
                    message = "Account or password does not exist"
                });
            }
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var role = await _userManager.GetRolesAsync(user);
                try
                {

                    var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role.FirstOrDefault()!),
                };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                    };


                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    if (role.Contains("Admin"))
                    {
                        return Json(new { code = 208, message = "Success", red = "/admin/dashboard" });
                    }
                    else
                    {
                        return Json(new { code = 200, message = "Success", section = true });
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }


            }
            else
            {

                return Json(new { code = 400, message = "Account or password does not exist" });

            }

        }
        [HttpPost("tai-khoan/dang-ky-tai-khoan")]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            model.UserName = model.UserName.ToLower();

            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
            {
                return Json(new { code = 400, message = "The account already exists on the system" });

            }
            if (model.ConfirmPassword != model.PasswordHash)
            {
                return Json(new { code = 400, message = "Passwords are not the same" });
            }
            ApplicationUser user = model;
            user.Email = model.UserName;
            user.CreateDate = DateTime.Now;
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.TotalMoney = 0;
            var result = await _userManager.CreateAsync(user, model.PasswordHash);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                try
                {

                    var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Member"),
                };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                    };


                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    await _signInManager.SignInAsync(user, isPersistent: false);



                    return Json(new { code = 200, message = "Success", section = true });

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            foreach (var error in result.Errors)
            {

                return Json(new { code = 400, message = "Password must have at least 1 uppercase letter and special character" });

            }

            return Json(new { code = 400, message = "Check the fields again" });

        }

        [HttpGet("dang-xuat")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
         new AuthenticationProperties { RedirectUri = "/Home/Index" }
          );
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}
