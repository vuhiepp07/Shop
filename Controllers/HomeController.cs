using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class HomeController: BaseController{
        public HomeController(SiteProvider provider, AppDbContext dbContext) : base(provider, dbContext)
        {
        }

        public IActionResult Index(){
            return View();
        }
    }
}