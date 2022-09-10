using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class BrandController : BaseController
    {
        public BrandController(SiteProvider provider) : base(provider)
        {
        }

    }
}