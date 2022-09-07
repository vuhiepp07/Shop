using System.Data;
using Dapper;

namespace Shop.Models{
    public class ProductRepository : BaseRepository
    {
        public ProductRepository(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<Product> GetProducts(){
            return connection.Query<Product>("Select * from Product");
        }

        public Product GetProductById(int id){
            return connection.QuerySingleOrDefault<Product>("Select Product where ProductId = @Id", new{
                Id = id
            });
        }
    }
}