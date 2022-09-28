using System.Data;

namespace Shop.Models{
    public class CartRepository : BaseRepository
    {
        public CartRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }
    }
}