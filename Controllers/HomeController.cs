using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
    [ServiceFilter(typeof(NavbarFilter))]
    public class HomeController: BaseController{
        public HomeController(SiteProvider provider) : base(provider)
        {
        }

        private void FillDataToViewBag(IEnumerable<Product> products){
            HashSet<int> BrandIdList = new HashSet<int>();
            HashSet<int> DiscountIdList = new HashSet<int>();
            foreach (Product product in products)
            {
                BrandIdList.Add(product.BrandId);
                DiscountIdList.Add(product.ProductDiscountId);
            }
            IEnumerable<Discount> discounts = provider.Discount.GetDiscountsByIdList(DiscountIdList);
            IEnumerable<Brand> brands = provider.Brand.GetBrandsByIdList(BrandIdList);
            ViewBag.Discounts = discounts;
            ViewBag.Brands = brands;
            ViewBag.Products = products;
        }

        public IActionResult Index(){
            IEnumerable<Product> products = provider.Product.GetProducts();
            FillDataToViewBag(products);
            return View();
        }

        [Route("/home/category/{categoryname:alpha}")]
        public IActionResult Category(string categoryname){
            Category category = provider.Category.GetCategoryByName(categoryname);
            IEnumerable<Product> products = provider.Product.GetProductsByCategoryId(category.CategoryId);
            FillDataToViewBag(products);
            return View();
        }

        [Route("/home/{brandname:alpha}/{categoryname:alpha}")]
        public IActionResult Brand(string brandname, string categoryname){
            Category category = provider.Category.GetCategoryByName(categoryname);
            Brand brand = provider.Brand.GetBrandByName(brandname);
            IEnumerable<Product> products = provider.Product.GetProductsByCategoryIdAndBrandId(category.CategoryId, brand.BrandId);
            FillDataToViewBag(products);
            return View();
        }
    }
}