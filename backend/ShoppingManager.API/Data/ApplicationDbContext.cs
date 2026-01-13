using Microsoft.EntityFrameworkCore;
using ShoppingManager.API.Models;

namespace ShoppingManager.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<Product> Products { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.Role).HasConversion<int>();
            });
            
            // LoginHistory configuration
            modelBuilder.Entity<LoginHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.LoginHistories)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.IPAddress).HasMaxLength(45);
                entity.Property(e => e.UserAgent).HasMaxLength(500);
            });
            
            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Code).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Unit).HasMaxLength(20).IsRequired();
                entity.Property(e => e.RefPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.Image).HasMaxLength(255);
                entity.Property(e => e.Description).HasColumnType("nvarchar(max)");
            });
            
            // Seed default admin user
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            var adminUser = new User
            {
                Id = 1,
                Email = "admin@shoppingmanager.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                FirstName = "System",
                LastName = "Administrator",
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            
            var purchaseUser = new User
            {
                Id = 3,
                Email = "purchase@shoppingmanager.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Purchase123!"),
                FirstName = "Purchase",
                LastName = "Manager",
                Role = UserRole.Purchase,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            
            modelBuilder.Entity<User>().HasData(adminUser, purchaseUser);
            
            // Seed sample products
            var sampleProducts = new[]
            {
                new Product
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Code = "LAPTOP001",
                    Name = "Dell Latitude 5520 Laptop",
                    Unit = "PC",
                    RefPrice = 1299.99m,
                    Description = "15.6-inch business laptop with Intel Core i7 processor",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Code = "MOUSE001",
                    Name = "Logitech MX Master 3 Mouse",
                    Unit = "PC",
                    RefPrice = 99.99m,
                    Description = "Advanced wireless mouse for productivity",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Code = "PAPER001",
                    Name = "A4 Copy Paper",
                    Unit = "REAM",
                    RefPrice = 4.99m,
                    Description = "500 sheets of white A4 copy paper",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Code = "PEN001",
                    Name = "Ballpoint Pen Set",
                    Unit = "SET",
                    RefPrice = 12.50m,
                    Description = "Set of 10 blue ballpoint pens",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Code = "MONITOR001",
                    Name = "Samsung 24-inch Monitor",
                    Unit = "PC",
                    RefPrice = 249.99m,
                    Description = "Full HD 1920x1080 LED monitor",
                    CreatedAt = DateTime.UtcNow
                }
            };
            
            modelBuilder.Entity<Product>().HasData(sampleProducts);
        }
    }
}