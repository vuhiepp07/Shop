using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Controllers;
using Shop.Models;

namespace Shop.Areas.Admin.Controllers{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UserController : BaseController
    {
        public UserController(SiteProvider provider) : base(provider)
        {
        }
    }
}