using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Controllers;
using Shop.Models;

namespace Shop.Areas.Admin.Controllers{
    [Area("Admin")]
    public class AuthController : BaseController
    {
        public AuthController(SiteProvider provider) : base(provider)
        {
        }

        public IActionResult Login(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel obj){
            User user = provider.User.Login(obj);
            
            if(user != null){
                Role userRole = provider.Role.getRoleById(user.RoleId);
                if(userRole.RoleName == "admin"){
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
                    return Redirect("/admin/home/index");
                }
                TempData["msg"] = "login failed";
            }
            TempData["msg"] = "login failed";
            return Redirect("/admin/auth/login");
        }

        //Handling when user choose to logout. After signout, return to Home page
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}