using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
    public class CartController : BaseController
    {
        private const string cartCode = "CartId";
        public CartController(SiteProvider provider) : base(provider)
        {
        }

         private string CheckCartCode(){
            string? cartId = Request.Cookies[cartCode];
            if(string.IsNullOrEmpty(cartId)){
                cartId = Helper.RandomString(64);
                Response.Cookies.Append(cartCode, cartId);
            }
            return cartId;
        }

        [ServiceFilter(typeof(NavbarFilter))]
        public IActionResult Index(){
            string cartId = CheckCartCode();
            ViewBag.CartDetail = provider.Cart.GetCarts(cartId);
            return View();
        }

        [HttpPost("/cart/add/{productId:int}")]
        public IActionResult Add(int productId){
            Cart obj = new Cart{
                ProductId = productId,
                Quantity = 1,
                CartId = CheckCartCode()
            };
            provider.Cart.Save(obj);
            return Json(provider.Cart.CountProducts(obj.CartId));
        }

        [HttpPost]
        public IActionResult Edit([FromBody] Cart obj){
            var EditSuccessMsg = new {status = "true"};
            var EditFailedMsg = new {status = "false"};
            string cartId = Request.Cookies[cartCode];
            obj.CartId = cartId;
            if(provider.Cart.Edit(obj) > 0){
                return Json(EditSuccessMsg);
            }
            return Json(EditFailedMsg);
        }

        [HttpPost]
        public IActionResult Delete(Cart obj){
            string cartId = Request.Cookies[cartCode];
            return Json(provider.Cart.Delete(obj));
        }

        [ServiceFilter(typeof(NavbarFilter))]
        public IActionResult CheckOut(){
            return View();
        }
    }
}