using System.Data;
using System.Data.SqlClient;

namespace Shop.Models{
    public class BaseProvider : IDisposable
    {
        IDbConnection connection;
        protected AppDbContext dbContext;
        protected IConfiguration configuration;
        public BaseProvider(IConfiguration configuration, AppDbContext dbContext){
            this.dbContext = dbContext;
            this.configuration = configuration;
        }
        protected IDbConnection Connection{
            get{
                if(connection is null){
                    connection = new SqlConnection(configuration.GetConnectionString("Shop"));
                }
                return connection;
            }
        }
        public void Dispose()
        {
            if(connection != null){
                connection.Dispose();
            }
            if(dbContext != null){
                dbContext.Dispose();
            }
        }
    }
}