using System.Data;

namespace Shop.Models{
    public class CartDetailRepository : BaseRepository
    {
        public CartDetailRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }
    }
}