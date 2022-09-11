using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shop.Controllers;
using Shop.Models;

namespace Shop.Areas.Admin.Controllers{
    [Area("Admin")]
    public class ProductController : BaseController
    {
        public ProductController(SiteProvider provider) : base(provider)
        {
        }

        void LoadData()
        {
            ViewBag.categories = provider.Category.GetCategories();
            ViewBag.brands = provider.Brand.GetBrands();
        }

        public IActionResult Index(){
            return View(provider.Product.GetProducts());
        }

        public IActionResult Create(){
            LoadData();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj){
            if(obj.ImageUrl != null){
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                using (WebClient client = new WebClient()){
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    string root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                    string ext = Path.GetExtension(obj.ImageUrl);
                    string filename = Helper.RandomString(64 - ext.Length) + ext;
                    client.DownloadFile(obj.ImageUrl, Path.Combine(root, filename));
                    obj.ImageUrl = filename;
                }

                // string root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                // string ext = Path.GetExtension(obj.ImageUrl);
                // string filename = Helper.RandomString(64 - ext.Length) + ext;
                // using (var client = new HttpClient()){
                //     client.DefaultRequestHeaders.ConnectionClose = true;
                //     using(var s = client.GetStreamAsync(obj.ImageUrl)){
                //         using(var fs = new FileStream(Path.Combine(root, filename), FileMode.Create)){
                //             s.Result.CopyTo(fs);
                //         }
                //     }
                //     obj.ImageUrl = filename;
            }
            

            if(provider.Product.CreateProduct(obj) > 0){
                provider.BrandCategory.Create(obj.BrandId, obj.CategoryId);
                return Redirect("/Admin/Product/Create");
            }
            else return Redirect("/Admin/Product/Error");
        }
    }
}