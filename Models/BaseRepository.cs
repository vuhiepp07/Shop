using System.Data;

namespace Shop.Models{
    /*Base repository contains IDbConnection and dbContext that help the inherit classes to access 
    them easily without have to create them */
    public abstract class BaseRepository{
        protected IDbConnection connection;
        protected AppDbContext dbContext;
        public BaseRepository(IDbConnection connection, AppDbContext dbContext){
            this.connection = connection;
            this.dbContext = dbContext;
        }
    }
}