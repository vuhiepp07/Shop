using System.Data;

namespace Shop.Models{
    public class RoleRepository : BaseRepository
    {
        public RoleRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public Role getRoleById(int id){
            return dbContext.Role.Where(p => p.RoleId == id).SingleOrDefault();
        }
    }
}