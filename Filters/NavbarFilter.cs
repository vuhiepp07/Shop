using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Models;

namespace Shop.Filters{
    public class NavbarFilter : Attribute, IActionFilter
    {
        SiteProvider provider;
        private const string cartCode = "CartId";

        public NavbarFilter(SiteProvider provider){
            this.provider = provider;
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
                string? cartId = con.Request.Cookies[cartCode];
                if(string.IsNullOrEmpty(cartId)){
                    cartId = Helper.RandomString(64);
                    con.Response.Cookies.Append(cartCode, cartId);
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