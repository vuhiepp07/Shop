using System.Data;
using Dapper;

namespace Shop.Models{
    public class OrderRepository : BaseRepository
    {
        public OrderRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public IEnumerable<Order> GetOrders(Guid userId){
            return  connection.Query<Order>("Select * from [Order] where UserId = @UserId", new{
                UserId = userId
            });
        }

        public int Create(Order obj){
            string sql = "Insert into [Order](OrderId, CreatedDate, UserId, UserNote, ReceiverAddress, ReceiverName, ReceiverPhone) values ";
            sql += "(@OrderId, @CreatedDate, @UserId, @UserNote, @ReceiverAddress, @ReceiverName, @ReceiverPhone)";
            return connection.Execute(sql, obj);
        }

        public int Cancel(string orderId){
            return connection.Execute("Update [Order] set Status = 0 where OrderId = @OrderId", new{
                OrderId = orderId
            });
        }
    }
}