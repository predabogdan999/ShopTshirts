﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShopTshirts;

namespace ShopTshirts.Migrations
{
    [DbContext(typeof(ProductsContext))]
    [Migration("20220203130607_newAMigration")]
    partial class newAMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ShopTshirts.Models.Categories", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("categoryName");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ShopTshirts.Models.Products", b =>
                {
                    b.Property<int>("productId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoriesId");

                    b.Property<int>("categoryId");

                    b.Property<string>("color");

                    b.Property<string>("description");

                    b.Property<int>("price");

                    b.Property<string>("productImg");

                    b.Property<string>("productName");

                    b.Property<int>("rating");

                    b.Property<int>("warranty");

                    b.HasKey("productId");

                    b.HasIndex("CategoriesId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ShopTshirts.Models.Products", b =>
                {
                    b.HasOne("ShopTshirts.Models.Categories", "Categories")
                        .WithMany("Products")
                        .HasForeignKey("CategoriesId");
                });
#pragma warning restore 612, 618
        }
    }
}
