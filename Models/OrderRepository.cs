using System.Data;
using Dapper;

namespace Shop.Models{
    public class OrderRepository : BaseRepository
    {
        public OrderRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public IEnumerable<Order> GetOrders(){
            return  connection.Query<Order>("Select * from Order");
        }

        public int Delete(int id){
            return connection.Execute("Delte from Oder where OrderId = @Id", new {Id = id});
        }
    }
}