using Shop.Models;

namespace Shop.Controllers{
    public class OrderController : BaseController
    {
        public OrderController(SiteProvider provider, AppDbContext dbContext) : base(provider, dbContext)
        {
        }
    }
}