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

        public Brand GetBrandByName(string brandName){
            return connection.QuerySingleOrDefault<Brand>("select * from Brand where BrandName = @Name", new{
                Name = brandName
            });
        }

        public Brand GetBrandById(int id){
            var result = from brand in dbContext.Brand
                            where brand.BrandId == id
                            select brand;
            return result.First();
        }

        public IEnumerable<Brand> GetBrandsByIdList(HashSet<int> BrandIdList){
            List<Brand> result = new List<Brand>();
            foreach (int BrandId in BrandIdList)
            {
                result.Add(GetBrandById(BrandId));
            }
            return result;
        }

        public int Create(Brand obj){
            dbContext.Brand.Add(obj);
            return dbContext.SaveChanges();
        }

        public int Edit(Brand obj){
            Brand result = dbContext.Brand.Where(p => p.BrandId == obj.BrandId).SingleOrDefault();
            result.BrandName = obj.BrandName;
            result.ImageUrl = obj.ImageUrl;
            result.Description = obj.Description;
            return dbContext.SaveChanges();
            // return connection.Execute("Update Brand set BrandName = @BrandName, ImageUrl = @ImageUrl, Description = @Description where BrandId = @BrandId", obj);
        }

        public int Delete(int id){
            Brand temp = GetBrandById(id);
            dbContext.Brand.Remove(temp);
            return dbContext.SaveChanges();
        }

        public IEnumerable<Category> GetBrandCategories(List<int> categoryIds){
            return connection.Query<Category>("Select * from Category where CategoryId in @CategoryList", new{
                CategoryList = categoryIds
            });
        }

    }
}