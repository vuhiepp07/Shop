using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
    [Authorize]
    public class OrderController : BaseController
    {
        public OrderController(SiteProvider provider) : base(provider)
        {
        }

        [ServiceFilter(typeof(NavbarFilter))]
        public IActionResult Index(){
            return View();
        }

        [ServiceFilter(typeof(NavbarFilter))]
        public IActionResult Create(){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<CheckOutProduct> products = provider.Cart.GetCheckOutProductList(userId);
            ViewBag.CheckoutProducts = products;
            return View();
        }

        
    }
}