using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Shop.Filters;
using Shop.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Shop"));
});
builder.Services.AddScoped(p => new SiteProvider(builder.Configuration, p.GetRequiredService<AppDbContext>()));
builder.Services.AddScoped(p => new NavbarFilter(p.GetRequiredService<SiteProvider>()));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(p => {
    p.LoginPath = "/auth/login";
    p.ExpireTimeSpan = TimeSpan.FromDays(7);
});
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapControllerRoute(name: "Admin", pattern: "{area:exists}/{controller=home}/{action=index}/{id?}");
app.Run();
