namespace Shop.Models{
    public class Cart{
        public int CartId {get; set;}
        public int TotalPrice {get; set;}
        public Guid UserId {get; set;}
        public virtual User User {get; set;}
        public virtual CartDetail CartDetail {get; set;}
    }
}