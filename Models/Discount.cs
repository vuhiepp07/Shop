using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models{
    //[Table("Discount")]
    public class Discount{
        //[Key]
        public int DiscountId {get; set;}

        //[Required]
        //[Column(TypeName = "nvarchar")]
        //[StringLength(30)]
        public string DiscountName {get; set;}

        //[Required]
        public string StartDate {get; set;}

        //[Required]
        public string EndDate {get; set;}

        //[Required]
        //[Column(TypeName ="float")]
        public double DiscountPercentage {get; set;}

        public string Description {get; set;}

        //[Required]
        public int Quantity {get; set;}

        //Collection navigation, lay nhung Product co Discount tuong ung
        public virtual List<Product> DiscountProducts {get; set;}
        public virtual List<Order> OrderDiscounts {get; set;}

    }
}