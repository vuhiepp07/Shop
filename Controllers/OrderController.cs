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
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            IEnumerable<Order> OrderList = provider.Order.GetOrders(userId);
            Dictionary<Order, List<OrderDetail>> mydict = new Dictionary<Order, List<OrderDetail>>();
            foreach(Order item in OrderList){
                mydict.Add(item, (List<OrderDetail>)provider.OrderDetail.GetOrderDetail(item.OrderId));
            }
            var sortedDict = from entry in mydict orderby entry.Key.CreatedDate descending select entry;
            ViewBag.OrdersDict = sortedDict;
            return View();
        }

        [ServiceFilter(typeof(NavbarFilter))]
        public IActionResult Create(){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<CheckOutProduct> products = provider.Cart.GetCheckOutProductList(userId);
            ViewBag.CheckoutProducts = products;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Order obj){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            obj.OrderId = userId + DateTime.UtcNow.AddHours(7).ToString();
            obj.UserId = Guid.Parse(userId);
            provider.Order.Create(obj);
            IEnumerable<CheckOutProduct> products = provider.Cart.GetCheckOutProductList(userId);
            List<int> ProductIdArr = new List<int>();
            foreach (CheckOutProduct item in products)
            {
                provider.OrderDetail.CreateDetail(new OrderDetail{
                    OrderId = obj.OrderId,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    SalePrice = item.DiscountPrice,
                    Quantity = item.Quantity,
                });
                ProductIdArr.Add(item.ProductId);
            }
            if(provider.Cart.Delete(userId, ProductIdArr.ToArray<int>()) > 0){
                return View("OrderResult", "Bạn đã đặt hàng thành công, vui lòng vào mục đơn hàng đã đặt trong menu cá nhân để kiểm tra tình trạng đơn hàng. Bạn sẽ được chuyển hướng về trang chủ sau 5 giây.");
            }
            else{
                return View("OrderResult", "Đặt hàng thất bại, vui lòng vào giỏ hàng và thử lại. Bạn sẽ được chuyển đến trang chủ sau 5 giây");
            }
        }

        [HttpPost]
        public IActionResult Cancel([FromBody] string orderId){
            var SuccessMsg = new {status = "true"};
            var FailedMsg = new {status = "false"};
            if(provider.Order.Cancel(orderId) >0){
                return Json(SuccessMsg);
            }
            return Json(FailedMsg);
            // return Json(provider.Order.Cancel(orderId));
        }

        public IActionResult OrderResult(){
            return View();
        }
        
    }
}