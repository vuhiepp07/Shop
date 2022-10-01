namespace Shop.Models{
    public class Cart{
        public string CartId {get; set;}
        public int ProductId {get; set;}
        public int Quantity {get; set;}
        public bool isSelectedToBuy {get; set;}

        public virtual Product Product {get; set;}
    }
}
