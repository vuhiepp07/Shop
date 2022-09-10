using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class BaseController:Controller{
        protected SiteProvider provider;
        public BaseController(SiteProvider provider){
            this.provider = provider;
        }
    }
}