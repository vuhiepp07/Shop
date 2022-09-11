using Microsoft.AspNetCore.Mvc;
using Shop.Controllers;
using Shop.Models;

namespace Shop.Areas.Admin.Controllers{
    [Area("Admin")]
    public class DiscountController : BaseController
    {
        public DiscountController(SiteProvider provider) : base(provider)
        {
        }

        public IActionResult Index(){
            return View(provider.Discount.GetDiscounts());
        }

        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        public IActionResult Create(Discount obj){
            if(provider.Discount.Create(obj) > 0){
                return Redirect("/Admin/Discount/Create");
            }
            else return Redirect("/Admin/Discount/Error");
        }
    }
}