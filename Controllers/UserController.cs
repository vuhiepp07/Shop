using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
    [Authorize]
    public class UserController : BaseController
    {
        public UserController(SiteProvider provider) : base(provider)
        {
        }

        [ServiceFilter(typeof(NavbarFilter))]
        public IActionResult Information(){
            ViewBag.User = provider.User.GetUserInfo(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return View();
        }

        [HttpPost]
        public IActionResult ChangeInformation([FromBody] User obj){
            var SuccessMsg = new {status = "true"};
            var FailedMsg = new {status = "false"};
            obj.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(provider.User.UpdateInformation(obj) > 0){
                return Json(SuccessMsg);
            }
            return Json(FailedMsg);
        }

        [HttpPost]
        public IActionResult UploadAvatar(IFormFile f){
            string root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user");
            if(f != null){
                string extension = Path.GetExtension(f.FileName);
                string filename = Helper.RandomString(64 - extension.Length) + extension;
                using(Stream fileStream = new FileStream(Path.Combine(root, filename), FileMode.Create)){
                    f.CopyTo(fileStream);
                }
                if(provider.User.UpdateAvatarImg(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), filename) > 0){
                    return Redirect("/user/information");
                }
            }
            return Redirect("User/error");
        }
    }
}