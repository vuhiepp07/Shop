namespace Shop.Models
{
    public class Category{
        public int CategoryId {get; set;}
        public string CategoryName {get; set;}
        public string Description {get; set;}

        public virtual List<Product> CategoryProducts {get; set;}

        public virtual BrandCategory BrandCategory  {get; set;}
    }
}