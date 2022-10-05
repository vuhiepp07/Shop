using System.Data;
using Dapper;

namespace Shop.Models{
    public class OrderDetailRepository : BaseRepository
    {
        public OrderDetailRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public int CreateDetail(OrderDetail obj){
            dbContext.OrderDetail.Add(obj);
            return dbContext.SaveChanges();
        }

        public IEnumerable<OrderDetail> GetOrderDetail(string OrderId){
            return connection.Query<OrderDetail>("Select * from OrderDetail where OrderId = @OrderId", new{
                OrderId = OrderId
            });
        }
    }
}