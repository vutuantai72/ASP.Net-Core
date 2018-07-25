using Microsoft.EntityFrameworkCore;
using ShopBaby.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBaby.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<FileImage> FileImages { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Product>().ToTable("Product");
            //modelBuilder.Entity<ProductCategory>()
            //    .ToTable("ProductCategory");
            modelBuilder.Entity<ProductCategory>().Property(b => b.CreatedDate)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Product>().Property(b => b.CreatedDate)
                .HasDefaultValueSql("getdate()");
        }
    }
}
