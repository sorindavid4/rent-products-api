﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using rent_products_api.DBContexts;

namespace rent_products_api.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20231122205356_update blob")]
    partial class updateblob
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("rent_products_api.DataLayer.Models.Payments.Payment", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<bool>("PaymentConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PaymentStatus")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("PaymentTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserPayingId")
                        .HasColumnType("char(36)");

                    b.HasKey("PaymentId");

                    b.HasIndex("UserPayingId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("rent_products_api.DataLayer.Models.Product.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("PricePerDay")
                        .HasColumnType("int");

                    b.Property<int>("PricePerHour")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("rent_products_api.DataLayer.Models.Product.ProductImage", b =>
                {
                    b.Property<Guid>("ProductImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<byte[]>("Data")
                        .HasColumnType("longblob");

                    b.Property<string>("ImageExtension")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ImageTitle")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("char(36)");

                    b.HasKey("ProductImageId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("rent_products_api.DataLayer.Models.Rent", b =>
                {
                    b.Property<Guid>("RentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ConfirmationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeSpan>("EndingHour")
                        .HasColumnType("time(6)");

                    b.Property<Guid?>("PaymentId")
                        .HasColumnType("char(36)");

                    b.Property<int>("PaymentType")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("RejectionTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RentType")
                        .HasColumnType("int");

                    b.Property<Guid>("RentedByUserId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("RentedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeSpan>("StartingHour")
                        .HasColumnType("time(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("RentId");

                    b.HasIndex("PaymentId");

                    b.HasIndex("ProductId");

                    b.HasIndex("RentedByUserId");

                    b.ToTable("Rents");
                });

            modelBuilder.Entity("rent_products_api.Models.BaseUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("FirstName");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("LastName");

                    b.Property<string>("Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("PasswordReset")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ResetToken")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("Verified")
                        .HasColumnType("datetime(6)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasDiscriminator<int>("UserType");
                });

            modelBuilder.Entity("rent_products_api.Models.User.AdminUser", b =>
                {
                    b.HasBaseType("rent_products_api.Models.BaseUser");

                    b.Property<string>("NumarTelefon")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("NumarTelefon");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("rent_products_api.Models.User.User", b =>
                {
                    b.HasBaseType("rent_products_api.Models.BaseUser");

                    b.Property<int>("Age")
                        .HasColumnType("int")
                        .HasColumnName("Age");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("PhoneNumber");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("rent_products_api.DataLayer.Models.Payments.Payment", b =>
                {
                    b.HasOne("rent_products_api.Models.User.User", "UserPaying")
                        .WithMany("Payments")
                        .HasForeignKey("UserPayingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserPaying");
                });

            modelBuilder.Entity("rent_products_api.DataLayer.Models.Product.ProductImage", b =>
                {
                    b.HasOne("rent_products_api.DataLayer.Models.Product.Product", null)
                        .WithMany("Images")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("rent_products_api.DataLayer.Models.Rent", b =>
                {
                    b.HasOne("rent_products_api.DataLayer.Models.Payments.Payment", "Payment")
                        .WithMany()
                        .HasForeignKey("PaymentId");

                    b.HasOne("rent_products_api.DataLayer.Models.Product.Product", "Product")
                        .WithMany("Rents")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("rent_products_api.Models.User.User", "RentedByUser")
                        .WithMany("Rents")
                        .HasForeignKey("RentedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");

                    b.Navigation("Product");

                    b.Navigation("RentedByUser");
                });

            modelBuilder.Entity("rent_products_api.Models.BaseUser", b =>
                {
                    b.OwnsMany("rent_products_api.DataLayer.Utils.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("char(36)");

                            b1.Property<Guid>("AccountUserId")
                                .HasColumnType("char(36)");

                            b1.Property<DateTime>("Created")
                                .HasColumnType("datetime(6)");

                            b1.Property<string>("CreatedByIp")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<DateTime>("Expires")
                                .HasColumnType("datetime(6)");

                            b1.Property<string>("ReplacedByToken")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<DateTime?>("Revoked")
                                .HasColumnType("datetime(6)");

                            b1.Property<string>("RevokedByIp")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<string>("Token")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.HasKey("Id");

                            b1.HasIndex("AccountUserId");

                            b1.ToTable("Users_RefreshTokens");

                            b1.WithOwner("Account")
                                .HasForeignKey("AccountUserId");

                            b1.Navigation("Account");
                        });

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("rent_products_api.DataLayer.Models.Product.Product", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("Rents");
                });

            modelBuilder.Entity("rent_products_api.Models.User.User", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Rents");
                });
#pragma warning restore 612, 618
        }
    }
}
