using System.Data;
using Dapper;

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

        public int ChangePassword(Guid UserId, ChangePasswordModel obj){
            return connection.Execute("Update [User] set Password = @NewPassword where Username = @Username and Password = @OldPassword and UserId = @UserId", new{
                UserId = UserId,
                Username = obj.Username,
                NewPassword = Helper.Hash(obj.Username + "^@#%!@(!&^$" + obj.NewPassword),
                OldPassword = Helper.Hash(obj.Username + "^@#%!@(!&^$" + obj.Password),
            });
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
            return connection.Execute("Update [User] set ImageUrl = @ImgUrl where UserId = @Id", new{
                Id = UserId,
                ImgUrl = ImgUrl
            });
        }
    }
}