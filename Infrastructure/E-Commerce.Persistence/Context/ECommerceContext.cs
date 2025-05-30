using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Context
{
    public class ECommerceContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=YCLGAMER;database=DbECommerce;integrated security=true; TrustServerCertificate = True");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentCard> PaymentCards { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCard> ShoppingCards { get; set; }
        public DbSet<ShoppingCardItem> ShoppingCardItems { get; set; }
        public DbSet<FavoritesCard> FavoritesCards { get; set; }
        public DbSet<FavoritesCardItem> FavoritesCardItems { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Quarter> Quarters { get; set; }
        public DbSet<OrderPaymentCard> OrderPaymentCards { get; set; }
        public DbSet<OrderPaymentOther> OrderPaymentOthers { get; set; }
        public DbSet<OrderPaymentAddress> OrderPaymentAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
    .HasOne(o => o.Order)
    .WithMany(oi => oi.OrderItems)
    .HasForeignKey(o => o.OrderID)
    .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
