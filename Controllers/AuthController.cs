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

        //Handling when user login, update Claims information and return login status
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel obj){
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
            if(provider.User.Register(obj) > 0){
                return Json(new {status = "true"});
            }
            return Json(new {status = "false"});
        }

        //Handling when user change their password, return if it success or not
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel obj){
            if(provider.User.ChangePassword(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), obj) > 0){
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Json(new {status = "true"});
            }
            return Json(new {status = "false"});
        }

        //Update user information (Mail, Address, Phone,......)
        public IActionResult UpdateInformation(User obj){
            return Json(provider.User.UpdateInformation(obj));
        }
    }
}