using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class HomeController: BaseController{
        public HomeController(SiteProvider provider) : base(provider)
        {
        }

        public IActionResult Index(){
            return View();
        }
    }
}