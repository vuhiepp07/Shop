using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
    [Authorize(Roles = "customer")]
    public class CartController : BaseController
    {
        public CartController(SiteProvider provider) : base(provider)
        {
        }

        //Access to user's cart pages
        [ServiceFilter(typeof(NavbarFilter))]
        public IActionResult Index(){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.CartDetail = provider.Cart.GetCarts(userId);
            provider.Cart.ResetCheckoutProductList(userId);
            ViewBag.Title = "Giỏ hàng";
            return View();
        }

        //Handling when user add products to cart
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

        //Handling when user edit the number of products in their cart
        [HttpPost]
        public IActionResult Edit([FromBody] Cart obj){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            obj.CartId = userId;
            if(provider.Cart.Edit(obj) > 0){
                return Json(new {status = "true"});
            }
            return Json(new {status = "false"});
        }

        //Handling when user delete permanently some products in their cart
        [HttpPost]
        public IActionResult Delete([FromBody] int[] ProductIdArr){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(provider.Cart.Delete(userId, ProductIdArr) > 0){
                return Json(new {status = "true"});
            }
            return Json(new {status = "false"});
        }

        //When user choose to checkout, get the selected products that they want to buy finally in the cart page
        [ServiceFilter(typeof(NavbarFilter))]
        [HttpPost]
        public IActionResult UpdateCheckoutProductList([FromBody] int[] ProductIdArr){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(provider.Cart.UpdateCheckoutProductList(userId, ProductIdArr) > 0){
                return Json(new {status = "true"});
            }
            return Json(new {status = "false"});
        }

    }
}