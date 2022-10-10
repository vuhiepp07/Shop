using Microsoft.AspNetCore.Mvc;
using Shop.Filters;
using Shop.Models;

namespace Shop.Controllers{
    public class CategoryController : BaseController
    {
        int size = 20;
        public CategoryController(SiteProvider provider) : base(provider)
        {
        }

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

        //Handling when user access to a category page which contains only products of that category
        [ServiceFilter(typeof(NavbarFilter))]
        [Route("/Product/{categoryname:alpha}/index/{page?}")]
        public IActionResult Index(string categoryname, int page = 1){
            Category category = provider.Category.GetCategoryByName(categoryname);
            IEnumerable<Product> products = provider.Product.GetProductsByCategoryId(page, size, category.CategoryId);
            FillDataToViewBag(products);
            int totalProduct = provider.Product.CountProductsInCategory(category.CategoryId);
            int totalPage = totalProduct % size == 0 ? totalProduct/size : totalProduct/size +1;
            ViewBag.totalPage = totalPage;
            ViewBag.Title = "Sản phẩm";
            return View();
        }

        //Handling when user access to a Category/Brand page which contains only products of that category with the chosen brand
        //For example: Laptop/Asus
        [ServiceFilter(typeof(NavbarFilter))]
        [Route("/Product/{categoryname:alpha}/{brandname:alpha}/{page?}")]
        public IActionResult Brand(string categoryname, string brandname, int page = 1){
            Category category = provider.Category.GetCategoryByName(categoryname);
            Brand brand = provider.Brand.GetBrandByName(brandname);
            IEnumerable<Product> products = provider.Product.GetProductsByCategoryIdAndBrandId(page, size, category.CategoryId, brand.BrandId);
            FillDataToViewBag(products);
            int totalProduct = provider.Product.CountProductsInCategoryAndBrand(category.CategoryId, brand.BrandId);
            int totalPage = totalProduct % size == 0 ? totalProduct/size : totalProduct/size +1;
            ViewBag.totalPage = totalPage;
            ViewBag.Title = "Sản phẩm";
            return View();
        }
    }
}