using Microsoft.AspNetCore.Mvc;
using Shop.Controllers;
using Shop.Models;

namespace Shop.Areas.Admin.Controllers{
    [Area("Admin")]
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
        public IActionResult Create(Brand obj, IFormFile f){
            string root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if(f != null){
                string extension = Path.GetExtension(f.FileName);
                string filename = Helper.RandomString(128 - extension.Length) + extension;
                using(Stream stream = new FileStream(Path.Combine(root, filename), FileMode.Create)){
                    f.CopyTo(stream);
                }
                obj.ImageUrl = filename;
            }

            if(provider.Brand.Create(obj) > 0){
                return Redirect("/Admin/Brand/Create");
            }
            else return Redirect("/Admin/Brand/Error");
        }
    }
}