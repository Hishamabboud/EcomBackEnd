﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ecommerence.Repository.Contexts;

namespace Ecommerence.Repository.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ecommerence.Domain.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime?>("Approved");

                    b.Property<DateTime?>("Canceled");

                    b.Property<DateTime?>("Commited");

                    b.Property<DateTime>("Created");

                    b.Property<Guid?>("CustomerId");

                    b.Property<Guid?>("EditorId");

                    b.Property<int>("Status");

                    b.Property<string>("Ticket");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EditorId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Ecommerence.Domain.Models.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<Guid?>("OrderId");

                    b.Property<Guid?>("ProductId");

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("Ecommerence.Domain.Models.PictureUrl", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<Guid?>("ProductId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("PictureUrl");
                });

            modelBuilder.Entity("Ecommerence.Domain.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.HasKey("Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Ecommerence.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<int>("Profile");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Ecommerence.Domain.Models.Order", b =>
                {
                    b.HasOne("Ecommerence.Domain.Models.User", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("Ecommerence.Domain.Models.User", "Editor")
                        .WithMany()
                        .HasForeignKey("EditorId");
                });

            modelBuilder.Entity("Ecommerence.Domain.Models.OrderItem", b =>
                {
                    b.HasOne("Ecommerence.Domain.Models.Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderId");

                    b.HasOne("Ecommerence.Domain.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Ecommerence.Domain.Models.PictureUrl", b =>
                {
                    b.HasOne("Ecommerence.Domain.Models.Product")
                        .WithMany("Pictures")
                        .HasForeignKey("ProductId");
                });
#pragma warning restore 612, 618
        }
    }
}
