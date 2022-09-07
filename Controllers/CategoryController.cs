using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class CategoryController : BaseController
    {
        public CategoryController(SiteProvider provider, AppDbContext dbContext) : base(provider, dbContext)
        {
        }
    }
}