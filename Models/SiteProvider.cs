namespace Shop.Models{
    public class SiteProvider : BaseProvider
    {
        public SiteProvider(IConfiguration configuration, AppDbContext dbContext) : base(configuration, dbContext)
        {
        }
        BrandRepository brand;
        public BrandRepository Brand{
            get{
                if(brand is null){
                    brand = new BrandRepository(Connection, dbContext);
                }
                return brand;
            }
        }

        BrandCategoryRepository brandCategory;
        public BrandCategoryRepository BrandCategory{
            get{
                if(brandCategory is null){
                    brandCategory = new BrandCategoryRepository(Connection, dbContext);
                }
                return brandCategory;
            }
        }

        CartRepository cart;
        public CartRepository Cart{
            get{
                if(cart is null){
                    cart = new CartRepository(Connection, dbContext);
                }
                return cart;
            }
        }

        CategoryRepository category;
        public CategoryRepository Category{
            get{
                if(category is null){
                    category = new CategoryRepository(Connection, dbContext);
                }
                return category;
            }
        }

        DiscountRepository discount;
        public DiscountRepository Discount{
            get{
                if(discount is null){
                    discount = new DiscountRepository(Connection, dbContext);
                }
                return discount;
            }
        }

        OrderRepository order;
        public OrderRepository Order{
            get{
                if(order is null){
                    order = new OrderRepository(Connection, dbContext);
                }
                return order;
            }
        }

        ProductRepository product;

        public ProductRepository Product{
            get{
                if(product is null){
                    product = new ProductRepository(Connection, dbContext);
                }
                return product;
            }
        }
    }
}