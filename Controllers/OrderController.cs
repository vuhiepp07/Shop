using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
    public class OrderController : BaseController
    {
        private const string cartCode = "CartId";
        public OrderController(SiteProvider provider) : base(provider)
        {
        }

        [ServiceFilter(typeof(NavbarFilter))]
        public IActionResult Index(){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<CheckOutProduct> products = provider.Cart.GetCheckOutProductList(userId);
            ViewBag.CheckoutProducts = products;
            return View();
        }
    }
}