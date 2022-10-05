namespace Shop.Models{
    public class OrderDetail{
        public string OrderId {get; set;}
        public int ProductId {get; set;}
        public string ProductName {get; set;}
        public int SalePrice {get; set;}
        public int Quantity {get; set;}
        public virtual Product Product {get; set;}
        public virtual Order Order {get; set;}
    }
}