using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopBaby.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBaby.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Product>().ToTable("Product");
            //modelBuilder.Entity<ProductCategory>()
            //    .ToTable("ProductCategory");
            modelBuilder.Entity<ProductCategory>().Property(b => b.CreatedDate)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Product>().Property(b => b.CreatedDate)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<OrderDetail>().HasKey(p => new { p.OrderID, p.ProductID});

            modelBuilder.Entity<OrderDetail>()
            .HasOne(pt => pt.Order)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(pt => pt.OrderID);

            modelBuilder.Entity<Order>()
            .HasOne(pt => pt.Customer)
            .WithMany(p => p.Orders)
            .HasForeignKey(pt => pt.CustomerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
