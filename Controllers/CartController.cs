using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class CartController : BaseController
    {
        public CartController(SiteProvider provider) : base(provider)
        {
        }

        public IActionResult Index(){
            return View();
        }
    }
}