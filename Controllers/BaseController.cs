using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    //A base controller class that has SiteProvide variable (which was injected as program service in program.cs) for it child classes to inherit
    public class BaseController:Controller{
        protected SiteProvider provider;
        public BaseController(SiteProvider provider){
            this.provider = provider;
        }
    }
}