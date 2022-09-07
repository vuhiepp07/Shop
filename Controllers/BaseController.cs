using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class BaseController:Controller{
        protected SiteProvider provider;
        protected AppDbContext dbContext;
        public BaseController(SiteProvider provider, AppDbContext dbContext){
            this.dbContext = dbContext;
            this.provider = provider;
        }
    }
}