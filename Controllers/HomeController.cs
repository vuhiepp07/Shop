using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers{
    public class HomeController: BaseController{
        public HomeController(SiteProvider provider) : base(provider)
        {
        }

        public IActionResult Index(){
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
            IEnumerable<Product> products = provider.Product.GetProducts();
            IEnumerable<Discount> discounts = provider.Discount.GetDiscounts();
            IEnumerable<Brand> brands = provider.Brand.GetBrands();
            ViewBag.Categories = categories;
            ViewBag.BrandOfCategories = myDict;
            ViewBag.Products = products;
            ViewBag.Discounts = discounts;
            ViewBag.Brands = brands;
            return View();
        }
    }
}