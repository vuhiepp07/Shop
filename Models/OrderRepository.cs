using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<OrderDetail> GetOrderDetail(string id){
            return connection.Query<OrderDetail>("Select * from OrderDetail where OrderId = @Id", new{
                Id = id
            });
        }

        public Order GetOrderById(string id){
            return connection.QuerySingleOrDefault<Order>("Select * from [Order] where OrderId = @id", new{
                id = id
            });
        }

        public int Create(Order obj){
            string sql = "Insert into [Order](OrderId, CreatedDate, UserId, UserNote, ReceiverAddress, ReceiverName, ReceiverPhone) values ";
            sql += "(@OrderId, @CreatedDate, @UserId, @UserNote, @ReceiverAddress, @ReceiverName, @ReceiverPhone)";
            return connection.Execute(sql, obj);
        }

        public int Cancel(string orderId){
            string sql = "Update [Order] set Status = 0 where OrderId = @OrderId";
            return dbContext.Database.ExecuteSqlRaw(sql, new SqlParameter("@OrderId", orderId));
            // return connection.Execute("Update [Order] set Status = 0 where OrderId = @OrderId", new{
            //     OrderId = orderId
            // });
        }
    }
}