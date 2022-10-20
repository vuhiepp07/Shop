using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;

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
            // return connection.Execute("Update Cart set Quantity = @Quantity where CartId = @Id and ProductId = @ProductId", new{
            //     Quantity = obj.Quantity,
            //     Id = obj.CartId,
            //     ProductId = obj.ProductId
            // });

            return connection.Execute("EditCart", new{
                Quantity = obj.Quantity,
                Id = obj.CartId,
                ProductId = obj.ProductId
            }, commandType:CommandType.StoredProcedure);
        }

        public int UpdateCheckoutProductList(string id, int[] ProductIdArr){
            return connection.Execute("Update Cart set isSelectedToBuy = 1 where CartId = @id and ProductId in @ProductIdArr", new{
                Id = id,
                ProductIdArr = ProductIdArr
            });
        }

        public IEnumerable<CheckOutProduct> GetCheckOutProductList(string id){
            string sql = "Select Product.ProductId, Product.ProductName, Product.DiscountPrice, Product.Price, Product.ImageUrl, Cart.CartId, Cart.Quantity ";
            sql +=  "from Cart join Product on Cart.ProductId = Product.ProductId where CartId = @id and isSelectedToBuy = 1";
            return connection.Query<CheckOutProduct>(sql, new{
                id = id
            });
        }

        public int Delete(string id, int[] ProductIdArr){
            var result = from cart in dbContext.Cart
                        where cart.CartId == id && ProductIdArr.Contains(cart.ProductId)
                        select cart;
            dbContext.Cart.RemoveRange(result);
            return dbContext.SaveChanges();
            // return connection.Execute("Delete Cart where CartId = @Id and ProductId in @ProductIdArr", new{
            //     Id = id,
            //     ProductIdArr = ProductIdArr
            // });
        }

        public int ResetCheckoutProductList(string id){
            return connection.Execute("Update Cart set isSelectedToBuy = 0 where CartId = @id", new{
                id = id
            });
        }
    }
}