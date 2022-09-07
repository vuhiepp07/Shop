using Microsoft.EntityFrameworkCore;


namespace Shop.Models{
    public class AppDbContext:DbContext{
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){

        }
        public DbSet<Brand> Brand {get; set;}
        public DbSet<BrandCategory> BrandCategory {get; set;}
        public DbSet<Cart> Cart {get; set;}
        public DbSet<CartDetail> CartDetail {get; set;}
        public DbSet<Category> Category {get; set;}
        public DbSet<Discount> Discount {get; set;}
        public DbSet<Order> Order {get; set;}
        public DbSet<Product> Product {get; set;}
        public DbSet<User> User {get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder builder){
            base.OnConfiguring(builder);
            builder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);
            builder.Entity<Category>(entity => {
                entity.ToTable("Category");
                entity.HasKey(p=> p.CategoryId);
                entity.Property(p => p.CategoryName).IsRequired(true);
            });

            builder.Entity<Brand>(entity => {
                entity.ToTable("Brand");
                entity.HasKey(p => p.BrandId);
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
            });

            builder.Entity<User>(entity => {
                entity.ToTable("User");
                entity.HasKey(p => p.UserId);
                entity.Property(p => p.Username).IsRequired(true);
                entity.Property(p => p.Password);
            });

            builder.Entity<Cart>(entity => {
                entity.ToTable("Cart");
                entity.HasKey(p => p.CartId);
                entity.Property(p=> p.UserId);
                entity.HasOne(p => p.User)
                    .WithOne( p => p.Cart)
                    .HasForeignKey<Cart>(p => p.UserId);
            });

            builder.Entity<CartDetail>(entity => {
                entity.ToTable("CartDetail");
                entity.HasKey(p => new {p.CartId, p.ProductId});
                entity.HasOne(p => p .Cart)
                    .WithOne(p => p.CartDetail)
                    .HasForeignKey<CartDetail>(p => p.CartId);
                entity.HasOne(p => p.Product)
                    .WithOne(p => p.CartDetail)
                    .HasForeignKey<CartDetail>(p => p.ProductId);
            });

            builder.Entity<Discount>(entity =>{
                entity.ToTable("Discount");
                entity.HasKey(p => p.DiscountId);
            });

            builder.Entity<Order>(entity =>{
                entity.ToTable("Order");
                entity.HasKey(p => p.OrderId);
                entity.HasOne(p => p.Discount)
                    .WithMany(p => p.OrderDiscounts)
                    .HasForeignKey(p => p.DiscountId);
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
            });
        }
    }
}