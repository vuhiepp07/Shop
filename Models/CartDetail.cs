namespace Shop.Models{
    public class CartDetail{
        public string CartId {get; set;}
        public int ProductId {get; set;}
        public int Quantity {get; set;}
        public string ProductName {get; set;}
        public int DiscountPrice {get; set;}
        public int Price {get; set;}
        public string Description {get; set;}
        public string ImageUrl {get; set;}
    }
}