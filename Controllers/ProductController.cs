using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class ProductController : BaseController
    {
        public ProductController(SiteProvider provider) : base(provider)
        {
        }

        public IActionResult GetProducts(){
            
            return View();
        }
    }
}