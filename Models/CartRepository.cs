using System.Data;
using Dapper;

namespace Shop.Models{
    public class CartRepository : BaseRepository
    {
        public CartRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public int Save(Cart obj){
            return connection.Execute("SaveCart", new{
                CartId = obj.CartId,
                ProductId = obj.ProductId,
                Quantity = obj.Quantity
            }, commandType:CommandType.StoredProcedure);
        }

        public int CountProducts(string cartId){
            return connection.ExecuteScalar<int>("Select Sum(Quantity) from Cart where CartId = @Id", new{
                id = cartId
            });
        }

        public IEnumerable<CartDetail> GetCarts(string id){
            return connection.Query<CartDetail>("Select Cart.CartId, Cart.ProductId, Cart.Quantity, Product.ProductName, Product.DiscountPrice, Product.Price, Product.Description, Product.ImageUrl from Cart join Product on Cart.ProductId = Product.ProductId and CartId = @id", new{
                id = id
            });
        }

        public int Edit(Cart obj){
            return connection.Execute("Update Cart set Quantity = @Quantity where CartId = @Id and ProductId = @ProductId", new{
                Quantity = obj.Quantity,
                Id = obj.CartId,
                ProductId = obj.ProductId
            });
        }

        public int Delete(Cart obj){
            return connection.Execute("Delete * from Cart where CartId = @Id and ProductId = @ProductId", new{
                Id = obj.CartId,
                ProductId = obj.ProductId
            });
        }
    }
}