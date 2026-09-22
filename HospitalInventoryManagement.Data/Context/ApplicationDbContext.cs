using HospitalInventoryManagement.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HospitalInventoryManagement.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Veritabanı Tabloları
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<MonthlyStatistics> MonthlyStatistics { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<TestTemplate> TestTemplates { get; set; }
        public DbSet<ProductRequest> ProductRequests { get; set; }
        public DbSet<ErrorLogs> ErrorLogs { get; set; }
        public DbSet<Sayim> Sayims { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);  // Identity tabloları için yapılandırma

            // Kategori ve Ürün (Category <--> Product) ilişkilendirmesi
            builder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);  // Kategori silindiğinde ilgili ürünler de silinir.

            // Ürün ve Ürün Tipi (Product <--> ProductType) ilişkilendirmesi
            builder.Entity<ProductType>()
                .HasMany(pt => pt.Products)
                .WithOne(p => p.ProductType)
                .HasForeignKey(p => p.TypeID)
                .OnDelete(DeleteBehavior.Restrict);  // Ürün tipi silinirse, ilişkili ürünler silinmez.

            // Ürün ve Platform (Product <--> Platform) ilişkilendirmesi
            builder.Entity<Platform>()
                .HasMany(pl => pl.Products)
                .WithOne(p => p.Platform)
                .HasForeignKey(p => p.PlatformID)
                .OnDelete(DeleteBehavior.Restrict);  // Platform silinirse, ilişkili ürünler silinmez.

            // Hastane ve Stok (Hospital <--> Stock) ilişkilendirmesi
            builder.Entity<Hospital>()
                .HasMany(h => h.Stocks)
                .WithOne(s => s.Hospital)
                .HasForeignKey(s => s.HospitalID)
                .OnDelete(DeleteBehavior.Cascade);  // Hastane silindiğinde ilgili stoklar da silinir.

            // Ürün ve Stok (Product <--> Stock) ilişkilendirmesi
            builder.Entity<Product>()
                .HasMany(p => p.Stocks)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductID)
                .OnDelete(DeleteBehavior.Cascade);  // Ürün silindiğinde ilgili stoklar da silinir.

            // Stok ve Stok İşlemleri (Stock <--> StockTransaction) ilişkilendirmesi
            builder.Entity<Stock>()
                .HasMany(s => s.StockTransactions)
                .WithOne(st => st.Stock)
                .HasForeignKey(st => st.StockID)
                .OnDelete(DeleteBehavior.Cascade);  // Stok silindiğinde ilgili işlemler de silinir.

            var adminRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER" }
            );

            // Admin kullanıcı tanımlanıyor
            var adminUserId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<ApplicationUser>();
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN@OZEMMEDIKAL.COM",
                Email = "admin@ozemmedikal.com",
                NormalizedEmail = "ADMIN@OZEMEDIKAL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123!")
            };

            builder.Entity<ApplicationUser>().HasData(adminUser);

            // Admin kullanıcısına rol atanıyor
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });

            builder.Entity<ProductRequest>()
                .HasOne(pr => pr.RequestedByUser)
                .WithMany()
                .HasForeignKey(pr => pr.RequestedByUserID)
                .OnDelete(DeleteBehavior.Restrict); // Silme davranışı

            // Stok işlemini yapan kullanıcı (kim girdi takibi). Navigation tutmuyoruz, sadece FK.
            builder.Entity<StockTransaction>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(st => st.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Sayım dönemi (Sayim) <--> Hastane : Restrict (çoklu cascade yolunu önlemek için)
            builder.Entity<Sayim>()
                .HasOne(z => z.Hospital)
                .WithMany()
                .HasForeignKey(z => z.HospitalID)
                .OnDelete(DeleteBehavior.Restrict);

            // Stok <--> Sayım : Restrict (sayım silinse bile stok satırı silinmez)
            builder.Entity<Stock>()
                .HasOne(s => s.Sayim)
                .WithMany(z => z.Stocks)
                .HasForeignKey(s => s.SayimID)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}