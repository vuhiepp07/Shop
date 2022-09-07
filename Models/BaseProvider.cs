using System.Data;
using System.Data.SqlClient;

namespace Shop.Models{
    public class BaseProvider : IDisposable
    {
        IDbConnection connection;
        protected IConfiguration configuration;
        public BaseProvider(IConfiguration configuration){
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
            if(connection is null){
                connection.Dispose();
            }
        }
    }
}