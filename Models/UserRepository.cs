using System.Data;

namespace Shop.Models{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }
    }
}