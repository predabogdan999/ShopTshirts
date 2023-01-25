using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopTshirts.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ShopTshirts.Identity;

namespace ShopTshirts
{
    public class ProductsContext : IdentityDbContext<IdentityUser>
    {
        public ProductsContext(DbContextOptions<ProductsContext> options)
           : base(options)
        {
            
        }
        public DbSet<Products> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Categories> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Products>().ToTable("Products");
            modelBuilder.Entity<Categories>().ToTable("Categories");
            
        }
    }
}