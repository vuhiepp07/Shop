using System.Data;

namespace Shop.Models{
    public abstract class BaseRepository{
        protected IDbConnection connection;
        protected AppDbContext dbContext;
        public BaseRepository(IDbConnection connection, AppDbContext dbContext){
            this.connection = connection;
            this.dbContext = dbContext;
        }
    }
}