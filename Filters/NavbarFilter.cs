using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Models;

namespace Shop.Filters{
    //Filter which help fill data to the navbar in every page
    public class NavbarFilter : Attribute, IActionFilter
    {
        SiteProvider provider;
        
        //IHttpContextAccessor variable which help to access to the User attribute which is only exist in Controllers
        //This help us to get the information of the user and fill them to the navbar
        IHttpContextAccessor accessor;

        public NavbarFilter(SiteProvider provider, IHttpContextAccessor accessor){
            this.provider = provider;
            this.accessor = accessor;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Controller is Controller con){
                List<Category> categories = provider.Category.GetCategories();
                Dictionary<int, List<Brand>> myDict = new Dictionary<int, List<Brand>>();
                foreach(Category category in categories){
                    IEnumerable<int> BrandIds = provider.BrandCategory.GetBrandsByCategoryId(category.CategoryId);
                    List<Brand> brandList = new List<Brand>();
                    foreach(int brandId in BrandIds){
                        Brand brand = provider.Brand.GetBrandById(brandId);
                        brandList.Add(brand);
                    }
                    myDict.Add(category.CategoryId, brandList);
                }
                string cartId = accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(accessor.HttpContext.User.Identity.IsAuthenticated){
                    con.ViewBag.User = provider.User.GetUserInfo(Guid.Parse(accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                }
                con.ViewBag.CartProductsNum = provider.Cart.CountProducts(cartId); 
                con.ViewBag.Categories = categories;
                con.ViewBag.BrandOfCategories = myDict;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}