using Microsoft.EntityFrameworkCore;
using RentCar.Web.Data;
using RentCar.Web.Services.Interfaces;
using RentCar.Web.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build(); 

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();         
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
