using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models{
    //[Table("Brand")]
    public class Brand{
        //[Key]
        public int BrandId {get; set;}

        //[Required]
        //[Column(TypeName ="nvarchar")]
        //[StringLength(100)]
        public string Description {get; set;}

        //[Required]
        //[Column(TypeName = "nvarchar")]
        //[StringLength(50)]
        public string BrandName {get; set;}
        public string ImageUrl {get; set;}

        //Colection navigation, lay ra tat ca nhung Category ma Brand nay co
        //public virtual List<Category> Categories {get; set;} 

        public virtual List<Product> BrandProducts {get; set;} 
        public virtual BrandCategory BrandCategory {get; set;}
    }
}