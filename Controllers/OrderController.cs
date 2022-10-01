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
            string cartId = Request.Cookies[cartCode];
            IEnumerable<CheckOutProduct> products = provider.Cart.GetCheckOutProductList(cartId);
            ViewBag.CheckoutProducts = products;
            return View();
        }
    }
}