using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class AuthController : BaseController
    {
        const int MemberRoleId = 2;
        IConfiguration configuration;
        public AuthController(SiteProvider provider, IConfiguration configuration) : base(provider)
        {
            this.configuration = configuration;
        }

        //Handling when user login, update Claims information and return login status
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel obj){
            User user = provider.User.Login(obj);
            
            if(user != null){
                Role userRole = provider.Role.getRoleById(user.RoleId);
                List<Claim> claims = new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Gender, user.Gender.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, userRole.RoleName)
                };
                
                if(!string.IsNullOrEmpty(user.DateOfBirth)){
                    claims.Add(new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth));
                }
                if(!string.IsNullOrEmpty(user.Phone)){
                    claims.Add(new Claim(ClaimTypes.MobilePhone, user.Phone));
                }
                if(!string.IsNullOrEmpty(user.Email)){
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                }
                if(!string.IsNullOrEmpty(user.FullName)){
                    claims.Add(new Claim(ClaimTypes.GivenName, user.FullName));
                }
                if(!string.IsNullOrEmpty(user.ImageUrl)){
                    claims.Add(new Claim(ClaimTypes.Uri, user.ImageUrl));
                }
                if(!string.IsNullOrEmpty(user.Address)){
                    claims.Add(new Claim(ClaimTypes.StreetAddress, user.Address));
                }
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                AuthenticationProperties properties = new AuthenticationProperties{
                    IsPersistent = obj.Remember
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Json(new {status = "true"});
            }
            return Json(new {status = "false"});
        }

        //Handling when user choose to logout. After signout, return to Home page
        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        //Handling when user register for a new account, return register status
        [HttpPost]
        public IActionResult Register([FromBody] User obj){
            obj.RoleId = MemberRoleId;
            if(provider.User.Register(obj) > 0){
                return Json(new {status = "true"});
            }
            return Json(new {status = "false"});
        }

        //Handling when user change their password, return if it success or not
        [HttpPost]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel obj){
            if(provider.User.ChangePassword(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), obj) > 0){
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Json(new {status = "true"});
            }
            return Json(new {status = "false"});
        }

        // Handling when user forget their password and want to reset password 
        [HttpPost]
        public IActionResult ResetPassword([FromBody] AuthModel obj){
            string resetResult = provider.User.ResetPassword(obj);
            if(resetResult != "failed"){
                string bodyMsg = "";
                bodyMsg += "<h2>Hệ thống vừa thực hiện reset mật khẩu của bạn theo yêu cầu</h2>";
                bodyMsg += $"<div>Mật khẩu mới của bạn là: <b>{resetResult}</b> vui lòng đăng nhập bằng mật khẩu mới, bạn có thể thực hiện đổi mật khẩu theo mong muốn trong option <b>Đổi mật khẩu</b> của trang web.</div>";
                string sendEmailResult = Helper.SendEmails(new EmailMessage{
                    Subject = "Reset mật khẩu thành công",
                    EmailTo = obj.Email,
                    Content = bodyMsg
                }, configuration);
                if(sendEmailResult == "Send mail success"){
                    return Json(new {status = "true"});
                }
                else{
                    return Json(new {status = $"Hệ thống gửi mail đang gặp trục trặc, lỗi 'Vượt quá giới hạn mail có thể gửi trong ngày' nên không thể gửi mail mật khẩu mới cho bạn. Vui lòng thử lại sau."});
                }
            }
            return Json(new {status = "false"});
        }

        //Update user information (Mail, Address, Phone,......)
        [Authorize(Roles = "customer")]
        public IActionResult UpdateInformation(User obj){
            return Json(provider.User.UpdateInformation(obj));
        }
    }
}