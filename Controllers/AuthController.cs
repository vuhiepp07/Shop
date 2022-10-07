using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class AuthController : BaseController
    {
        public AuthController(SiteProvider provider) : base(provider)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel obj){

            var LoginSuccessMsg = new {status = "true"};
            var LoginFailedMsg = new {status = "false"};
            User user = provider.User.Login(obj);
            
            if(user != null){
                List<Claim> claims = new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Gender, user.Gender.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
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
                return Json(LoginSuccessMsg);
            }
            return Json(LoginFailedMsg);
        }

        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult Register([FromBody] User obj){
            var RegisterSuccessMsg = new {status = "true"};
            var RegisterFailedMsg = new {status = "false"};
            if(provider.User.Register(obj) > 0){
                return Json(RegisterSuccessMsg);
            }
            return Json(RegisterFailedMsg);
        }

        public IActionResult ChangePassword(ChangePasswordModel obj){
            return Json(provider.User.ChangePassword(obj));
        }

        public IActionResult UpdateInformation(User obj){
            return Json(provider.User.UpdateInformation(obj));
        }

    }
}