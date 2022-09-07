namespace Shop.Models{
    public class Order{
        public int OrderId {get; set;}
        public string ReceiveAddress {get; set;}
        public string CreatedDate {get; set;}
        public int TotalPrice {get; set;}
        public string UserNote {get; set;}
        public string ReceiverName {get; set;}
        public string Status {get; set;}
        public int DiscountId {get; set;}
        public virtual Discount Discount {get; set;}

    }
}