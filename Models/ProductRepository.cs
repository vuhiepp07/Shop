using System.Data;
using Dapper;

namespace Shop.Models{
    public class ProductRepository : BaseRepository
    {
        public ProductRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public IEnumerable<Product> GetProducts(){
            return connection.Query<Product>("Select * from Product");
        }

        public Product GetProductById(int id){
            return connection.QuerySingleOrDefault<Product>("Select * from Product where ProductId = @Id", new{
                Id = id
            });
        }

        public IEnumerable<Product> GetProductsByCategoryId(int id){
            using(dbContext){
                var result = from product in dbContext.Product
                                where product.CategoryId == id
                                select product;
                return result.ToList<Product>();
            }
        }

        public IEnumerable<Product> GetProductByBrandId(int id){
            return connection.Query<Product>("Select * form Product where Product.BrandId = @Id", new{
                Id = id
            });
        }

        public IEnumerable<Product> GetProductsByDiscountId(int id){
            using(dbContext){
                var result = from product in dbContext.Product
                                where product.ProductDiscountId == id
                                select product;
                return result.ToList<Product>();
            }
        }

        public int CreateProduct(Product obj){
            string sql = "Insert into Product(Productname, Quantity, Price, Description, ImageUrl, CategoryId, BrandId, ProductDiscountId)" + 
                "values(@ProductName, @Quantity, @Price, @Description, @ImageUrl, @CategoryId, @BrandId, @ProductDiscountId)";
                
            return connection.Execute(sql, new{
                    ProductName = obj.ProductName,
                    Quantity = obj.Quantity,
                    Price = obj.Price,
                    Description = obj.Description,
                    ImageUrl = obj.ImageUrl,
                    CategoryId = obj.CategoryId,
                    BrandId = obj.BrandId,
                    ProductDiscountId = obj.ProductDiscountId
                 });
        }
        
    }
}