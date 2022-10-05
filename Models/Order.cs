namespace Shop.Models{
    public class Order{
        public string OrderId {get; set;}
        public DateTime CreatedDate {get; set;}
        public Guid UserId {get; set;}
        public string UserNote {get; set;}
        public string ReceiverAddress {get; set;}
        public string ReceiverName {get; set;}
        public string ReceiverPhone {get; set;}
        public int Status {get; set;}
        public virtual User User {get; set;}
        public virtual List<OrderDetail> OrderDetailList {get; set;}
    }
}