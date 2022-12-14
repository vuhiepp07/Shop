using Microsoft.EntityFrameworkCore;


namespace Shop.Models{
    public class AppDbContext:DbContext{
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){
        }
        public DbSet<Brand> Brand {get; set;}
        public DbSet<BrandCategory> BrandCategory {get; set;}
        public DbSet<Cart> Cart {get; set;}
        public DbSet<Category> Category {get; set;}
        public DbSet<Discount> Discount {get; set;}
        public DbSet<Order> Order {get; set;}
        public DbSet<OrderDetail> OrderDetail {get; set;}
        public DbSet<Product> Product {get; set;}
        public DbSet<User> User {get; set;}
        public DbSet<Role> Role {get; set;}
        public DbSet<MailSender> MailSender {get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder builder){
            base.OnConfiguring(builder);
            builder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);
            builder.Entity<MailSender>(entity =>{
                entity.ToTable("MailSender");
                entity.HasKey(p => p.usr);
            });

            builder.Entity<Category>(entity => {
                entity.ToTable("Category");
                entity.HasKey(p=> p.CategoryId);
                entity.Property(p => p.CategoryName).IsRequired(true);
                entity.Property(p => p.Description).IsRequired(false);
                entity.Property(p => p.CategoryId).ValueGeneratedOnAdd();
                entity.HasIndex(p => p.CategoryName).IsUnique(true);
            });

            builder.Entity<Brand>(entity => {
                entity.ToTable("Brand");
                entity.HasKey(p => p.BrandId);
                entity.Property(p => p.Description).IsRequired(false);
                entity.Property(p => p.ImageUrl).IsRequired(false);
                entity.Property(p => p.BrandId).ValueGeneratedOnAdd();
                entity.HasIndex(p => p.BrandName).IsUnique(true);

            });

            builder.Entity<BrandCategory>(entity => {
                entity.ToTable("BrandCategory");
                entity.HasKey(p => new {p.BrandId, p.CategoryId});
                entity.HasOne(p => p.Brand)
                    .WithOne(p => p.BrandCategory)
                    .HasForeignKey<BrandCategory>(p => p.BrandId);
                entity.HasOne(p => p.Category)
                    .WithOne(p => p.BrandCategory)
                    .HasForeignKey<BrandCategory>(p => p.CategoryId);
                entity.Property(p => p.NumOfProducts).HasDefaultValue(0);
                entity.HasIndex(p => p.BrandId).IsUnique(false);
                entity.HasIndex(p => p.CategoryId).IsUnique(false);
            });

            builder.Entity<User>(entity => {
                entity.ToTable("User");
                entity.HasKey(p => p.UserId);
                entity.Property(p => p.FullName).IsRequired(false);
                entity.Property(p => p.Email).IsRequired(false);
                entity.Property(p => p.DateOfBirth).IsRequired(false);
                entity.Property(p => p.UserId).HasDefaultValueSql("NEWID()");
                entity.Property(p => p.ImageUrl).IsRequired(false);
                entity.Property(p => p.Phone).IsRequired(false);
                entity.Property(p => p.Address).IsRequired(false);
                entity.HasIndex(p => p.Username).IsUnique(true);
                entity.HasOne(p => p.Role)
                    .WithMany(p => p.UsersInRole)
                    .HasForeignKey(p => p.RoleId);
            });

            builder.Entity<Role>(entity => {
                entity.ToTable("Role");
                entity.HasKey(p => p.RoleId);
            });

            builder.Entity<Cart>(entity => {
                entity.ToTable("Cart");
                entity.HasKey(p => new {p.CartId, p.ProductId});
                entity.HasIndex(p => p.CartId).IsUnique(false);
                entity.HasIndex(p => p.ProductId).IsUnique(false);
                entity.HasOne(p => p.Product)
                                .WithOne(p => p.Cart)
                                .HasForeignKey<Cart>(p => p.ProductId);
            });

            builder.Entity<Discount>(entity =>{
                entity.ToTable("Discount");
                entity.HasKey(p => p.DiscountId);
                entity.Property(p => p.DiscountId).ValueGeneratedOnAdd();
                entity.HasIndex(p => p.DiscountName).IsUnique(true);
                entity.Property(p => p.Description).IsRequired(false);
            });

            builder.Entity<Order>(entity =>{
                entity.ToTable("Order");
                entity.HasKey(p =>  p.OrderId);
                entity.Property(p => p.UserNote).IsRequired(false);
                entity.Property(p => p.Status).HasDefaultValue(1);
                entity.HasOne(p => p.User)
                            .WithMany(p => p.UserOrders)
                            .HasForeignKey(p => p.UserId);
            });

            builder.Entity<OrderDetail>(entity =>{
                entity.ToTable("OrderDetail");
                entity.HasKey(p =>  new{p.OrderId, p.ProductId});
                entity.HasOne(p => p.Product)
                                .WithOne(p => p.OrderDetail)
                                .HasForeignKey<OrderDetail>(p => p.ProductId);
                entity.HasOne(p => p.Order)
                            .WithMany(p => p.OrderDetailList)
                            .HasForeignKey(p => p.OrderId);
                entity.HasIndex(p => p.OrderId).IsUnique(false);
                entity.HasIndex(p => p.ProductId).IsUnique(false);
            });

            builder.Entity<Product>(entity => {
                entity.ToTable("Product");
                entity.HasKey(p => p.ProductId);
                entity.HasOne(p => p.Brand)
                    .WithMany(p => p.BrandProducts)
                    .HasForeignKey(p => p.BrandId);
                entity.HasOne(p => p.Category)
                    .WithMany(p => p.CategoryProducts)
                    .HasForeignKey(p => p.CategoryId);
                entity.HasOne(p => p.Discount)
                    .WithMany(p => p.DiscountProducts)
                    .HasForeignKey(p => p.ProductDiscountId);
                entity.Property(p => p.ProductDiscountId).HasDefaultValue(1);
                entity.Property(p => p.ProductId).ValueGeneratedOnAdd();
                entity.Property(p => p.DiscountPrice).HasDefaultValue(0);
            });
        }
    }
}