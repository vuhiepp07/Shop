using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class ProductController : BaseController
    {
        public ProductController(SiteProvider provider, AppDbContext dbContext) : base(provider, dbContext)
        {
        }

        public IActionResult GetProducts(){
            
            return View();
        }
    }
}