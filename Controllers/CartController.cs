using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
    [Authorize]
    public class CartController : BaseController
    {
        public CartController(SiteProvider provider) : base(provider)
        {
        }


        [ServiceFilter(typeof(NavbarFilter))]
        public IActionResult Index(){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.CartDetail = provider.Cart.GetCarts(userId);
            provider.Cart.ResetCheckoutProductList(userId);
            return View();
        }

        [HttpPost("/cart/add/{productId:int}")]
        public IActionResult Add(int productId){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart temp = new Cart{
                CartId = userId,
                ProductId = productId,
                Quantity = 1
            };
            provider.Cart.Save(temp);
            return Json(provider.Cart.CountProducts(userId));
        }

        [HttpPost]
        public IActionResult Edit([FromBody] Cart obj){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            obj.CartId = userId;
            var EditSuccessMsg = new {status = "true"};
            var EditFailedMsg = new {status = "false"};
            if(provider.Cart.Edit(obj) > 0){
                return Json(EditSuccessMsg);
            }
            return Json(EditFailedMsg);
        }

        [HttpPost]
        public IActionResult Delete([FromBody] int[] ProductIdArr){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var DeleteSuccessMsg = new {status = "true"};
            var DeleteFailedMsg = new {status = "false"};
            if(provider.Cart.Delete(userId, ProductIdArr) > 0){
                return Json(DeleteSuccessMsg);
            }
            return Json(DeleteFailedMsg);
        }


        [ServiceFilter(typeof(NavbarFilter))]
        [HttpPost]
        public IActionResult UpdateCheckoutProductList([FromBody] int[] ProductIdArr){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var SuccessMsg = new {status = "true"};
            var FailedMsg = new {status = "false"};
            if(provider.Cart.UpdateCheckoutProductList(userId, ProductIdArr) > 0){
                return Json(SuccessMsg);
            }
            return Json(FailedMsg);
        }

    }
}