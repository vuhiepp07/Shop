namespace Shop.Models{
    public class SiteProvider : BaseProvider
    {
        /* Inherit dbContext and Connection to the SQL server from BaseProvider.
        This class will contains the declaration of repository classes which will be inject to all the controller classes to help avoid
        create the same repository variable of the same repository class from times to times in different controllers. 
        The SqlConnection and dbContext attributes will also
        be injected to Repository classes to help avoid create new SqlConnection to the SQL server from times to times in the Repository classes */
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

        RoleRepository role;
        public RoleRepository Role{
            get{
                if(role is null){
                    role = new RoleRepository(Connection, dbContext);
                }
                return role;
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

        UserRepository user;
        public UserRepository User{
            get{
                if(user is null){
                    user = new UserRepository(Connection, dbContext);
                }
                return user;
            }
        }

        OrderDetailRepository orderDetail;
        public OrderDetailRepository OrderDetail{
            get{
                if(orderDetail is null){
                    orderDetail = new OrderDetailRepository(Connection, dbContext);
                }
                return orderDetail;
            }
        }
    }
}