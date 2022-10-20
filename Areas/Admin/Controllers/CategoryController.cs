using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Controllers;
using Shop.Models;

namespace Shop.Areas.Admin.Controllers{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CategoryController : BaseController
    {
        public CategoryController(SiteProvider provider) : base(provider)
        {
        }

        public IActionResult Index(){
            return View(provider.Category.GetCategories());
        }

        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj){
            if(provider.Category.Create(obj) > 0){
                return Redirect("/Admin/Category/Create");
            }
            else return Redirect("/Admin/Category/Error");
        }
    }
}