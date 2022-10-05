namespace Shop.Models{
    public class CheckOutProduct{
        public int ProductId {get; set;}
        public string ProductName {get; set;}
        public int DiscountPrice {get; set;}
        public int Price {get; set;}
        public string ImageUrl {get; set;}
        public string CartId {get; set;}
        public int Quantity {get; set;}
    }
}