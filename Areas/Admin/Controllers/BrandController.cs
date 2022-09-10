using Microsoft.AspNetCore.Mvc;
using Shop.Controllers;
using Shop.Models;

namespace Shop.Areas.Admin.Controllers{
    public class BrandController : BaseController
    {
        public BrandController(SiteProvider provider) : base(provider)
        {
        }

        public IActionResult Index(){
            return View(provider.Brand.GetBrands());
        }

        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand obj){
            if(provider.Brand.Create(obj) > 1){
                return Redirect("/Brand");
            }
            else return Redirect("/Brand/Error");
        }
    }
}