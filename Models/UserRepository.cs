using System.Data;
using Dapper;

namespace Shop.Models{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public User Login(AuthModel obj){
            return connection.QuerySingleOrDefault<User>("Select UserId, Username, Email, FullName, Phone, Gender, DateOfBirth, ImageUrl from [User] where Password = @Password and (Username = @Username or Email = @Username)", new{
                Username = obj.Username,
                Password = Helper.Hash(obj.Username + "^@#%!@(!&^$" + obj.Password)
            });
        }

        public int Register(User obj){
            return connection.Execute("Insert into [User](Username, Password, Gender, ImageUrl) values(@Username, @Password, @Gender, @ImageUrl)", new{
                Username = obj.Username,
                Password = Helper.Hash(obj.Username + "^@#%!@(!&^$" + obj.Password),
                Gender = true,
                ImageUrl = "userDefault.png"
            });
        }

        public int ChangePassword(ChangePasswordModel obj){
            return connection.Execute("Update [User] set Password = @NewPassword where Username = @Username and Password = @OldPassword", new{
                Username = obj.Username,
                NewPassword = obj.NewPassword,
                OldPassword = obj.Password
            });
        }

        public int UpdateInformation(User obj){
            return connection.Execute("Update [User] set Email = @Email, FullName = @FullName, Phone = @Phone, Gender = @Gender, DateOfBirth = @DateOfBirth, ImageUrl = @ImageUrl where UserId = @Id", new{
                Email = obj.Email,
                FullName = obj.FullName,
                Phone = obj.Phone,
                Gender = obj.Gender,
                DateOfBirth = obj.DateOfBirth,
                ImageUrl = obj.ImageUrl
            });
        }
    }
}