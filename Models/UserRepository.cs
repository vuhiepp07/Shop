using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Shop.Models{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }
        public User GetUserInfo(Guid UserId){
            return connection.QuerySingleOrDefault<User>("Select Email, FullName, Phone, Gender, DateOfBirth, Address, ImageUrl from [User] where UserId = @Id", new{
                Id = UserId,
            });
        }

        public string GetUserImgUrl(Guid UserId){
            return connection.QuerySingleOrDefault<string>("Select ImageUrl from [User] where UserId = @UserId", new{
                UserId = UserId
            });
        }

        public User Login(AuthModel obj){
            return connection.QuerySingleOrDefault<User>("Login", new{
                Username = obj.Username,
                Password = Helper.Hash(obj.Username + "^@#%!@(!&^$" + obj.Password)
            }, commandType: CommandType.StoredProcedure);
        }

        public int Register(User obj){
            return connection.Execute("Insert into [User](Username, Password, Gender, Email, ImageUrl, RoleId, FullName, Phone, DateOfBirth, Address) values(@Username, @Password, @Gender, @Email, @ImageUrl, @RoleId, @FullName, @Phone, @DateOfBirth, @Address)", new{
                Username = obj.Username,
                Password = Helper.Hash(obj.Username + "^@#%!@(!&^$" + obj.Password),
                Gender = true,
                Email = obj.Email,
                ImageUrl = "userDefault.png",
                RoleId = obj.RoleId,
                FullName = "defaultname",
                Phone = " ",
                DateOfBirth = " ",
                Address = " "
            });
        }

        public int ChangePassword(Guid UserId, ChangePasswordModel obj){
            return connection.Execute("ChangeUserPassword", new{
                UserId = UserId,
                Username = obj.Username,
                NewPassword = Helper.Hash(obj.Username + "^@#%!@(!&^$" + obj.NewPassword),
                OldPassword = Helper.Hash(obj.Username + "^@#%!@(!&^$" + obj.Password),
            }, commandType: CommandType.StoredProcedure);
        }

        public int UpdateInformation(User obj){
            return connection.Execute("Update [User] set Email = @Email, FullName = @FullName, Phone = @Phone, Gender = @Gender, DateOfBirth = @DateOfBirth, Address = @Address where UserId = @Id", new{
                Id = obj.UserId,
                Email = obj.Email,
                FullName = obj.FullName,
                Phone = obj.Phone,
                Gender = obj.Gender,
                DateOfBirth = obj.DateOfBirth,
                Address = obj.Address
            });
        }

        public int UpdateAvatarImg(Guid UserId, string ImgUrl){
            string sql = "Update [User] set ImageUrl = @ImgUrl where UserId = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("@ImgUrl", ImgUrl),
                new SqlParameter("@Id", UserId)
            };
            return dbContext.Database.ExecuteSqlRaw(sql, parameters);
            // return connection.Execute("Update [User] set ImageUrl = @ImgUrl where UserId = @Id", new{
            //     Id = UserId,
            //     ImgUrl = ImgUrl
            // });
        }
    }
}