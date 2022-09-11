using Microsoft.EntityFrameworkCore;
using Shop.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("Shop");
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped(p => new SiteProvider(builder.Configuration, p.GetRequiredService<AppDbContext>()));
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapControllerRoute(name: "Admin", pattern: "{area:exists}/{controller=home}/{action=index}/{id?}");
app.Run();
