using System.Data;
using Dapper;

namespace Shop.Models{
    public class BrandRepository : BaseRepository
    {
        public BrandRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public IEnumerable<Brand> GetBrands(){
            return connection.Query<Brand>("Select * from Brand");
        }

        public Brand GetBrandById(int id){
            var result = from brand in dbContext.Brand
                            where brand.BrandId == id
                            select brand;
            return result.First();
        }

        public int Create(Brand obj){
            using(dbContext){
                dbContext.Brand.Add(obj);
                return dbContext.SaveChanges();
            }
        }

        public int Edit(Brand obj){
            return connection.Execute("Update Brand set BrandName = @BrandName, ImageUrl = @ImageUrl, Description = @Description where BrandId = @BrandId", obj);
        }

        public int Delete(int id){
            Brand temp = GetBrandById(id);
            using(dbContext){
                dbContext.Brand.Remove(temp);
                return dbContext.SaveChanges();
            }
        }

        public IEnumerable<Category> GetBrandCategories(List<int> categoryIds){
            return connection.Query<Category>("Select * from Category where CategoryId in @CategoryList", new{
                CategoryList = categoryIds
            });
        }

    }
}