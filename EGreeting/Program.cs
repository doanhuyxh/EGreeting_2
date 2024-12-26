using EGreeting.Data;
using EGreeting.Models;
using EGreeting.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllers();
builder.Services.AddTransient<ICommon, Common>();
builder.Services.AddScoped<VersionUpdate>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<UserManager<ApplicationUser>>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddTransient<IIdentityDataInitializer, IdentityDataInitializer>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.LoginPath = "/dang-nhap";
        options.ExpireTimeSpan = TimeSpan.FromDays(365 * 100);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/access-denied";
    });
var connectionString = builder.Configuration.GetConnectionString("connMSSQL") ?? throw new InvalidCastException(nameof(args));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    }));
var app = builder.Build();



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
      name: "Areas",
      areaName: "AdminGreenting",
      pattern: "{controller=AdminDashboard}/{action=Index}/{id?}");
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var initializer = services.GetRequiredService<IIdentityDataInitializer>();
//    await initializer.SeedData(
//        services.GetRequiredService<UserManager<ApplicationUser>>(),
//        services.GetRequiredService<RoleManager<IdentityRole>>()
//    );
//}
app.Run();
