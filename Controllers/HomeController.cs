using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
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

        private IEnumerable<Product> FilterProductByBrandIdAndCategoryName(int brandid, string categoryname){
            Category category = provider.Category.GetCategoryByName(categoryname);
            IEnumerable<Product> products = new List<Product>();
            if(brandid == 0){
                products = provider.Product.GetProductsByCategoryId(category.CategoryId);
            }
            else{
                products = provider.Product.GetProductsByCategoryIdAndBrandId(category.CategoryId, brandid);
            }
            return products;
        }

        private IEnumerable<Product> FilterProductByBrandId(int brandid){
            IEnumerable<Product> products = new List<Product>();
            if(brandid == 0){
                products = provider.Product.GetProducts();
            }
            else{
                products = provider.Product.GetProductByBrandId(brandid);
            }
            return products;
        }

        private IEnumerable<Product> FilterProductByPriceRange(IEnumerable<Product> tempProducts, int pricerangeid){
            int min = 0;
            int max = 0;
            switch(pricerangeid){
                case 1:
                    min = 0;
                    max = 1000000;
                    break;
                case 2:
                    min = 1000000;
                    max = 5000000;
                    break;
                case 3:
                    min = 5000000;
                    max = 10000000;
                    break;
                case 4:
                    min = 10000000;
                    max = 20000000;
                    break;
                case 5:
                    min = 20000000;
                    max = 2147483647;
                    break;
                default:
                    min = 0;
                    max = 2147483647;
                    break;
            }
            List<Product> products = new List<Product>();
            foreach(Product product in tempProducts){
                if(product.Price > min && product.Price < max){
                    products.Add(product);
                }
            }
            return products;
        }

        [ServiceFilter(typeof(NavbarFilter))]
        public IActionResult Index(){
            IEnumerable<Product> products = provider.Product.GetProducts();
            FillDataToViewBag(products);
            return View();
        }

        [ServiceFilter(typeof(NavbarFilter))]
        [Route("/home/category/{categoryname:alpha}")]
        public IActionResult Category(string categoryname){
            Category category = provider.Category.GetCategoryByName(categoryname);
            IEnumerable<Product> products = provider.Product.GetProductsByCategoryId(category.CategoryId);
            FillDataToViewBag(products);
            return View();
        }

        [ServiceFilter(typeof(NavbarFilter))]
        [Route("/home/{brandname:alpha}/{categoryname:alpha}")]
        public IActionResult Brand(string brandname, string categoryname){
            Category category = provider.Category.GetCategoryByName(categoryname);
            Brand brand = provider.Brand.GetBrandByName(brandname);
            IEnumerable<Product> products = provider.Product.GetProductsByCategoryIdAndBrandId(category.CategoryId, brand.BrandId);
            FillDataToViewBag(products);
            return View();
        }

        [HttpPost("home/filter/{brandid:int}/{pricerangeid:int}")]
        public IActionResult FilterProductByBrandAndPrice(int brandid, int pricerangeid){
            IEnumerable<Product> tempProducts = FilterProductByBrandId(brandid);
            return Json(FilterProductByPriceRange(tempProducts, pricerangeid));
        }

        [HttpPost("/home/filter/{brandid:int}/{categoryname:alpha}/{pricerangeid:int}")]
        public IActionResult FilterProductByCategoryBrandPrice(int brandid, string categoryname, int pricerangeid){
            IEnumerable<Product> tempProducts = FilterProductByBrandIdAndCategoryName(brandid, categoryname);
            return Json(FilterProductByPriceRange(tempProducts, pricerangeid));
        }
    }
}