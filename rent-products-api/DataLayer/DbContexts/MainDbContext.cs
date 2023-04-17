using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rent_products_api.DataLayer.Models;
using rent_products_api.DataLayer.Models.Payments;
using rent_products_api.DataLayer.Models.Product;
using rent_products_api.DataLayer.Utils;
using rent_products_api.Models;
using rent_products_api.Models.User;

namespace rent_products_api.DBContexts
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
           : base(options)
        {
        }
        public DbSet<BaseUser> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Users tables config

            modelBuilder.Entity<BaseUser>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<BaseUser>()
                .HasDiscriminator(u => u.UserType)
                .HasValue<AdminUser>(UserType.AdminUser)
                .HasValue<User>(UserType.SimpleUser);

            modelBuilder.Entity<AdminUser>()
                .Property(b => b.FirstName)
                .HasColumnName("FirstName");

            modelBuilder.Entity<User>()
                .Property(b => b.FirstName)
                .HasColumnName("FirstName");

            modelBuilder.Entity<AdminUser>()
                .Property(b => b.LastName)
                .HasColumnName("LastName");

            modelBuilder.Entity<User>()
                .Property(b => b.LastName)
                .HasColumnName("LastName");

            modelBuilder.Entity<AdminUser>()
                .Property(b => b.NumarTelefon)
                .HasColumnName("NumarTelefon");

            modelBuilder.Entity<User>()
                .Property(b => b.Age)
                .HasColumnName("Age");

            modelBuilder.Entity<User>()
               .Property(b => b.PhoneNumber)
               .HasColumnName("PhoneNumber");
            #endregion

            #region Product tables config
            modelBuilder.Entity<Product>()
                .HasKey(x => x.ProductId);

            modelBuilder.Entity<ProductImage>()
                .HasKey(x => x.ProductImageId);

            modelBuilder.Entity<Rent>()
                .HasKey(x => x.RentId);

            modelBuilder.Entity<Rent>()
                .HasOne(x => x.RentedByUser)
                .WithMany(x => x.Rents)
                .HasForeignKey(x => x.RentedByUserId);

            modelBuilder.Entity<Payment>()
                .HasOne(x => x.UserPaying)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.UserPayingId);

            modelBuilder.Entity<Rent>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Rents)
                .HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<Payment>()
                .HasKey(x => x.PaymentId);

            #endregion

            #region DataSeed

            #endregion
        }
    }
}
