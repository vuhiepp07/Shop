using Shop.Controllers;
using Shop.Models;

namespace Shop.Areas.Admin.Controllers{
    public class UserController : BaseController
    {
        public UserController(SiteProvider provider) : base(provider)
        {
        }
    }
}