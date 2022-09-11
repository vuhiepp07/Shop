using System.Data;
using Dapper;

namespace Shop.Models{
    public class DiscountRepository : BaseRepository
    {
        public DiscountRepository(IDbConnection connection, AppDbContext dbContext) : base(connection, dbContext)
        {
        }

        public IEnumerable<Discount> GetDiscounts(){
            return connection.Query<Discount>("Select * from Discount");
        }

        public int Create(Discount obj){
            return connection.Execute("insert into Discount(DiscountName, DiscountPercentage, StartDate, EndDate, Quantity) values" +
            "(@DiscountName, @DiscountPercentage, @StartDate, @EndDate, @Quantity)", obj);
        }

        public Discount GetDiscountById(int id){
            using(dbContext){
            var dis = from discount in dbContext.Discount
                            where discount.DiscountId == id
                            select discount;
            return (Discount)dis;
            }
        }

        public int Delete(int id){
            Discount dis = GetDiscountById(id);
            using(dbContext){
                dbContext.Discount.Remove((Discount)dis);
                return dbContext.SaveChanges();
            }
        }

        public int Edit(Discount obj){
            Discount dis = GetDiscountById(obj.DiscountId);
            using(dbContext){
                dis.DiscountName = obj.DiscountName;
                dis.DiscountPercentage = obj.DiscountPercentage;
                dis.StartDate = obj.StartDate;
                dis.EndDate = obj.EndDate;
                dis.Quantity = obj.Quantity;
                return dbContext.SaveChanges();
            }
        }
    }
}