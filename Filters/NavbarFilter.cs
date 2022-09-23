using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Models;

namespace Shop.Filters{
    public class NavbarFilter : Attribute, IActionFilter
    {
        SiteProvider provider;
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
                con.ViewBag.Categories = categories;
                con.ViewBag.BrandOfCategories = myDict;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}