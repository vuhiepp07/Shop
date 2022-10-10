using System.Data;
using System.Data.SqlClient;

namespace Shop.Models{
    /*BaseProvider class contains AdbContext and Configuration attributes which were declared as services in program.cs file.
    This class also create the connection to SQL server so the SiteProvider class can inherit it */
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