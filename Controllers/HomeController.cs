using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
    public class HomeController: BaseController{
        int size = 20;
        public HomeController(SiteProvider provider) : base(provider)
        {
        }

        // private void FillDataToViewBag(IEnumerable<Product> products){
        //     HashSet<int> BrandIdList = new HashSet<int>();
        //     HashSet<int> DiscountIdList = new HashSet<int>();
        //     foreach (Product product in products)
        //     {
        //         BrandIdList.Add(product.BrandId);
        //         DiscountIdList.Add(product.ProductDiscountId);
        //     }
        //     IEnumerable<Discount> discounts = provider.Discount.GetDiscountsByIdList(DiscountIdList);
        //     IEnumerable<Brand> brands = provider.Brand.GetBrandsByIdList(BrandIdList);
        //     ViewBag.Discounts = discounts;
        //     ViewBag.Brands = brands;
        //     ViewBag.Products = products;
        // }

        private void FillDataToViewBag(IEnumerable<Product> products){
            HashSet<int> BrandIdList = new HashSet<int>();
            foreach (Product product in products)
            {
                BrandIdList.Add(product.BrandId);
            }
            IEnumerable<Brand> brands = provider.Brand.GetBrandsByIdList(BrandIdList);
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

        // Filter product function in the server-size, this is not more used, filter will be handled in the front end
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

        // [ServiceFilter(typeof(NavbarFilter))]
        // public IActionResult Index(){
        //     IEnumerable<Product> products = provider.Product.GetProducts();
        //     FillDataToViewBag(products);
        //     return View();
        // }

        [ServiceFilter(typeof(NavbarFilter))]
        [Route("{controller=home}/{action=index}/{page?}")]
        public IActionResult Index(int page = 1){
            IEnumerable<Product> products = provider.Product.GetProducts(page, size);
            FillDataToViewBag(products);
            int totalProduct = provider.Product.CountProducts();
            int totalPage = totalProduct % size == 0 ? totalProduct/size : totalProduct/size +1;
            ViewBag.totalPage = totalPage;
            return View();
        }

        // [ServiceFilter(typeof(NavbarFilter))]
        // [Route("/home/category/{categoryname:alpha}")]
        // public IActionResult Category(string categoryname){
        //     Category category = provider.Category.GetCategoryByName(categoryname);
        //     IEnumerable<Product> products = provider.Product.GetProductsByCategoryId(category.CategoryId);
        //     FillDataToViewBag(products);
        //     return View();
        // }

        // [ServiceFilter(typeof(NavbarFilter))]
        // [Route("/home/{brandname:alpha}/{categoryname:alpha}")]
        // public IActionResult Brand(string brandname, string categoryname){
        //     Category category = provider.Category.GetCategoryByName(categoryname);
        //     Brand brand = provider.Brand.GetBrandByName(brandname);
        //     IEnumerable<Product> products = provider.Product.GetProductsByCategoryIdAndBrandId(category.CategoryId, brand.BrandId);
        //     FillDataToViewBag(products);
        //     return View();
        // }

        // Filter product function in the server-size, this is not more used, filter will be handled in the front end
        [HttpPost("home/filter/{brandid:int}/{pricerangeid:int}")]
        public IActionResult FilterProductByBrandAndPrice(int brandid, int pricerangeid){
            IEnumerable<Product> tempProducts = FilterProductByBrandId(brandid);
            return Json(FilterProductByPriceRange(tempProducts, pricerangeid));
        }

        // Filter product function in the server-size, this is not more used, filter will be handled in the front end
        [HttpPost("/home/filter/{brandid:int}/{categoryname:alpha}/{pricerangeid:int}")]
        public IActionResult FilterProductByCategoryBrandPrice(int brandid, string categoryname, int pricerangeid){
            IEnumerable<Product> tempProducts = FilterProductByBrandIdAndCategoryName(brandid, categoryname);
            return Json(FilterProductByPriceRange(tempProducts, pricerangeid));
        }

        [ServiceFilter(typeof(NavbarFilter))]
        [HttpPost]
        public IActionResult searchProduct(string productName){
            IEnumerable<Product> products = provider.Product.SearchProductsByName(productName);
            FillDataToViewBag(products);
            return View();
        }

        [ServiceFilter(typeof(NavbarFilter))]
        [Route("home/productdetail/{productId:int}")]
        public IActionResult ProductDetail(int productId){
            Product prod = provider.Product.GetProductById(productId);
            ViewBag.MainProduct = prod;
            ViewBag.Products = provider.Product.GetProductInTheSameCategory(prod);
            return View();
        }

        
    }
}