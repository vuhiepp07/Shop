using System.Data;
using Dapper;

namespace Shop.Models{
    public class CategoryRepository : BaseRepository
    {
        public CategoryRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public IEnumerable<Category> GetCategories(){
            return dbContext.Category;
        }

        public Category GetCategoryById(int id){
            return connection.QueryFirstOrDefault<Category>("Select * from Category where CategoryId = @Id", new {Id = id});
        }

        public int Create(Category obj){
            using(dbContext){
                dbContext.Category.Add(obj);
                return dbContext.SaveChanges();
            }
        }

        public int Delete(int id){
            return connection.Execute("Delete from Category where CategoryId = @Id", new {Id = id});
        }

        public int Edit(Category obj){
            return connection.Execute("Update Category set CategoryName = @CategoryName, Description = @Description where CategoryId = @CategoryId", obj);
        }

        public IEnumerable<Brand> GetBrandsProvideCategory(List<int> BrandIds){
            using(dbContext){
                var result = from brand in dbContext.Brand
                                where BrandIds.Contains(brand.BrandId)
                                select brand;
                return result.ToList<Brand>();
            }
        }
    }
}