using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models{
    //[Table("Product")]
    public class Product{
        //[Key]
        public int ProductId {get; set;}

        //[Required]
        //[Column(TypeName = "nvarchar")]
        //[StringLength(50)]
        public string ProductName {get; set;}

        //[Required]
        public int Quantity {get; set;}
        public int Price {get; set;}

        //[Required]
        //[StringLength(1000)]
        //[Column(TypeName = "nvarchar")]
        public string Description {get; set;}

        //[Required]
        public string ImageUrl {get; set;}

        public int BrandId {get; set;}
        public int CategoryId {get; set;}
        public int ProductDiscountId {get; set;}

        //[ForeignKey("BrandId")]
        public virtual Brand Brand {get; set;}

        //[ForeignKey("CategoryId")]
        public virtual Category Category {get; set;}

        //[ForeignKey("DiscountId")]
        public virtual Discount Discount {get; set;}

        public virtual CartDetail CartDetail {get; set;}
    }
}