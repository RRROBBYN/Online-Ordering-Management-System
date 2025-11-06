using Microsoft.EntityFrameworkCore;
using _4IdiotsInc.Model;

namespace _4IdiotsInc.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<UserShippingAddress> UserShippingAddress { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserAccount>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserAccount>()
                .HasIndex(u => u.Username)
                .IsUnique();


            modelBuilder.Entity<Products>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Products>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Products>()
                .Property(p => p.IsAvailable)
                .HasDefaultValue(true);

            modelBuilder.Entity<Products>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ProductCategory>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<ProductCategory>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<ProductCategory>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);


            // Orders configuration
            modelBuilder.Entity<Orders>()
                .HasKey(o => o.Id);
            modelBuilder.Entity<Orders>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(12,2)");
            modelBuilder.Entity<Orders>()
                .Property(o => o.OrderDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Orders>()
                .HasOne(o => o.Status)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StatusId);
            modelBuilder.Entity<Orders>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);

            // OrderItems configuration
            modelBuilder.Entity<OrderItems>()
                .HasKey(oi => oi.Id);
            modelBuilder.Entity<OrderItems>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(10,2)");
            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // OrderStatus configuration
            modelBuilder.Entity<OrderStatus>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<OrderStatus>()
                .HasIndex(s => s.StatusName)
                .IsUnique();

            modelBuilder.Entity<UserShippingAddress>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<UserShippingAddress>()
                .Property(s => s.IsDefault)
                .HasDefaultValue(false);

            modelBuilder.Entity<UserShippingAddress>()
                .Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<UserShippingAddress>()
                .Property(s => s.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<UserShippingAddress>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
