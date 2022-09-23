using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;

namespace Shop.Models{
    public class BrandCategoryRepository : BaseRepository
    {
        public BrandCategoryRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public IEnumerable<BrandCategory> GetBrandCategories(){
            return connection.Query<BrandCategory>("Select * from BrandCategory");
        }

        public IEnumerable<int> GetBrandCategoriesId(int brandId){
            List<int> CategoryIdList = new List<int>();

            var kq = from brandCategory in dbContext.BrandCategory
                        group brandCategory by brandCategory.BrandId;
            foreach (var group in kq){
                if(group.Key == brandId){
                    foreach(var category in group){
                        CategoryIdList.Add(category.CategoryId);
                    }
                }
            }
            return CategoryIdList;
        }

        public int Create(int brandId, int CategoryId){
            return connection.Execute("AddBrandCategory", new{
                BrandId = brandId,
                CategoryId = CategoryId
            }, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<int> GetBrandsByCategoryId (int CategoryId){
            return connection.Query<int>("getBrandsByCategoryId", new{
                CategoryId = CategoryId
            }, commandType: CommandType.StoredProcedure);
           
        }
    }
}