using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
    public class CategoryController : BaseController
    {
        public CategoryController(SiteProvider provider) : base(provider)
        {
        }
    }
}