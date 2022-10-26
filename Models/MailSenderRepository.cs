using System.Data;
using Dapper;

namespace Shop.Models{
    public class MailSenderRepository : BaseRepository
    {
        public MailSenderRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public MailSender GetMailSender(){
            return connection.QueryFirstOrDefault<MailSender>("Select * from MailSender");
        }
    }
}