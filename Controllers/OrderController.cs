using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
    [Authorize(Roles = "customer")]
    public class OrderController : BaseController
    {
        IConfiguration configuration;
        public OrderController(SiteProvider provider, IConfiguration configuration) : base(provider)
        {   
            this.configuration = configuration;
        }

        //Return the index page in which has all the user order information
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
            ViewBag.Title = "Đơn hàng của tôi";
            return View();
        }

        //Get products in the user checkout ProductList, return the view of Create order page
        [ServiceFilter(typeof(NavbarFilter))]
        public IActionResult Create(){
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<CheckOutProduct> products = provider.Cart.GetCheckOutProductList(userId);
            ViewBag.CheckoutProducts = products;
            ViewBag.Title = "Đặt mua hàng";
            return View();
        }

        // public int SendEmails(EmailMessage obj){
        //     IConfiguration section = configuration.GetSection("Mails:Gmail");
        //     using (SmtpClient client = new SmtpClient(section.GetValue<string>("Host"), section.GetValue<int>("Port")){
        //         Credentials = new NetworkCredential(section.GetValue<string>("Email"), section.GetValue<string>("Password")), EnableSsl = true
        //     }){
        //         try{
        //             MailMessage message = new MailMessage(new MailAddress(section.GetValue<string>("Email"),"TheShop"), new MailAddress(obj.EmailTo)){
        //                 IsBodyHtml = true,
        //                 Subject = obj.Subject,
        //                 Body = obj.Content
        //             };
        //             client.Send(message);
        //             return 1;
        //         }
        //         catch(Exception ex){
        //             return 0;
        //         }
        //     }
        // }

        private string customizeEmailBody(bool order, IEnumerable<OrderDetail> products, Order obj){
            string bodyMsg = "";
            if(order == true){
                bodyMsg += "<h2>Chúc mừng bạn đã đặt hàng thành công, sau đây là thông tin đơn hàng của bạn</h2>";
            }
            else{
                bodyMsg += "<h2>Bạn đã hủy đơn hàng thành công, sau đây là thông tin đơn hàng đã hủy</h2>";
            }
            bodyMsg += $"<div><b>Mã đơn hàng:</b> {obj.OrderId}</div>@";
            bodyMsg += $"<div><b>Thời gian đặt hàng:</b> {obj.CreatedDate.ToString()}</div>@";
            bodyMsg += $"<div><b>Địa chỉ nhận hàng:</b> {obj.ReceiverAddress}</div>@";
            bodyMsg += $"<div><b>Tên người nhận hàng:</b> {obj.ReceiverName}</div>@";
            bodyMsg += "<h3 style=\"margin-top: 40px;\">Thông tin chi tiết đơn hàng</h3>";
            
            string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";  
            string htmlTableEnd = "</table>";  
            string htmlHeaderRowStart = "<tr style=\"background-color:#6FA1D2; color:#ffffff;\">";  
            string htmlHeaderRowEnd = "</tr>";  
            string htmlTrStart = "<tr style=\"color:#555555;\">";  
            string htmlTrEnd = "</tr>";  
            string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";  
            string htmlTdEnd = "</td>";  

            bodyMsg += htmlTableStart;  
            bodyMsg += htmlHeaderRowStart;  
            bodyMsg += htmlTdStart + "Tên sản phẩm" + htmlTdEnd;  
            bodyMsg += htmlTdStart + "Số lượng" + htmlTdEnd;  
            bodyMsg += htmlTdStart + "Giá bán" + htmlTdEnd;  
            bodyMsg += htmlHeaderRowEnd;  

            int totalPrice = 0;
            string totalPriceInString = "";

            foreach(OrderDetail item in products){
                bodyMsg += htmlTrStart;
                bodyMsg += htmlTdStart + item.ProductName + htmlTdEnd;
                bodyMsg += htmlTdStart + item.Quantity + htmlTdEnd;
                bodyMsg += htmlTdStart + Helper.normalizePrice(item.SalePrice.ToString())+ "đ" + htmlTdEnd;
                bodyMsg += htmlTrEnd;
                totalPrice += item.SalePrice * item.Quantity;
            }
            bodyMsg += htmlTableEnd;
            totalPriceInString = Helper.normalizePrice(totalPrice.ToString()) + "đ";
            bodyMsg += $"<h3 style=\"color:red; margin-top: 20px;\">Tổng giá trị đơn hàng: {totalPriceInString}</h3>";
            bodyMsg = bodyMsg.Replace("@", System.Environment.NewLine);
            return bodyMsg;
        }

        //Handling when user create a new order, return create order result
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
                IEnumerable<OrderDetail> orderDetails = provider.Order.GetOrderDetail(obj.OrderId);
                string bodyMsg = customizeEmailBody(true, orderDetails, obj);
                string sendEmailResult = Helper.SendEmails(provider.Mail.GetMailSender() ,new EmailMessage{
                    Subject = "Đặt hàng thành công",
                    EmailTo = User.FindFirstValue(ClaimTypes.Email),
                    Content = bodyMsg
                }, configuration);
                if(sendEmailResult == "Send mail success"){
                    return View("OrderResult", "Bạn đã đặt hàng thành công. Hệ thống sẽ gửi thông tin đơn hàng đến email của bạn ngay lập tức. Bạn có thể kiểm tra đơn hàng đã đặt trong menu cá nhân để kiểm tra tình trạng đơn hàng. Bạn sẽ được chuyển hướng về trang chủ sau 7 giây.");
                }
                else{
                    return View("OrderResult", $"Bạn đã đặt hàng thành công. Hệ thống gửi mail đang gặp trục trặc lỗi 'Vượt quá giới hạn mail có thể gửi trong ngày' nên không thể gửi mail đến bạn. Bạn có thể kiểm tra đơn hàng đã đặt trong menu cá nhân để kiểm tra tình trạng đơn hàng. Bạn sẽ được chuyển hướng về trang chủ sau 7 giây.");
                }
            }
            else{
                return View("OrderResult", "Đặt hàng thất bại, vui lòng vào giỏ hàng và thử lại. Bạn sẽ được chuyển đến trang chủ sau 7 giây");
            }
        }

        //Handling when user choose to cancel one of their orders, return cancel result
        [HttpPost]
        public IActionResult Cancel([FromBody] string orderId){
            if(provider.Order.Cancel(orderId) >0){
                Order obj = provider.Order.GetOrderById(orderId);
                IEnumerable<OrderDetail> products = provider.Order.GetOrderDetail(orderId);
                string bodyMsg = customizeEmailBody(false, products, obj);
                string sendEmailResult = Helper.SendEmails(provider.Mail.GetMailSender(), new EmailMessage{
                    Subject = "Hủy đặt hàng",
                    EmailTo = User.FindFirstValue(ClaimTypes.Email),
                    Content = bodyMsg
                }, configuration);
                if(sendEmailResult == "Send mail success"){
                    return Json(new {status = "true"});
                }
                else{
                    return Json(new {status = $"Hệ thống gửi mail đang gặp trục trặc, lỗi 'Vượt quá giới hạn mail có thể gửi trong ngày' nên không thể gửi mail thông tin hủy đơn hàng đến bạn. Tuy nhiên đơn hàng của bạn đã được hủy thành công."});
                }
            }
            return Json(new {status = "false"});
        }

        public IActionResult OrderResult(){
            return View();
        }
        
    }
}