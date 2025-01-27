﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using src.Infrastructure.Db;

#nullable disable

namespace EcommerceVibbra.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20240924205807_CompleteDev")]
    partial class CompleteDev
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("src.Domain.Entities.Bid", b =>
                {
                    b.Property<int>("BidId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("BidId"));

                    b.Property<bool>("Accepted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("DealId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("BidId");

                    b.HasIndex("DealId");

                    b.HasIndex("UserId");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("src.Domain.Entities.Deal", b =>
                {
                    b.Property<int>("DealId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("DealId"));

                    b.Property<string>("Description")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("TradeFor")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UrgencyType")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("DealId");

                    b.HasIndex("UserId");

                    b.ToTable("Deals");
                });

            modelBuilder.Entity("src.Domain.Entities.DealImage", b =>
                {
                    b.Property<int>("DealImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("DealImageId"));

                    b.Property<int>("DealId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("DealImageId");

                    b.HasIndex("DealId");

                    b.ToTable("DealImages");
                });

            modelBuilder.Entity("src.Domain.Entities.DealLocation", b =>
                {
                    b.Property<int>("DealLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("DealLocationId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("DealId")
                        .HasColumnType("int");

                    b.Property<double>("Lat")
                        .HasColumnType("double");

                    b.Property<double>("Lng")
                        .HasColumnType("double");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.Property<int>("ZipCode")
                        .HasColumnType("int");

                    b.HasKey("DealLocationId");

                    b.HasIndex("DealId")
                        .IsUnique();

                    b.ToTable("DealLocation");
                });

            modelBuilder.Entity("src.Domain.Entities.Delivery", b =>
                {
                    b.Property<int>("DeliveryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("DeliveryId"));

                    b.Property<int>("DealId")
                        .HasColumnType("int");

                    b.Property<decimal>("DeliveryPrice")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("DeliveryId");

                    b.HasIndex("DealId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("src.Domain.Entities.DeliverySteps", b =>
                {
                    b.Property<int>("DeliveryStepsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("DeliveryStepsId"));

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DeliveryId")
                        .HasColumnType("int");

                    b.Property<int>("DeliveryStatus")
                        .HasColumnType("int");

                    b.HasKey("DeliveryStepsId");

                    b.HasIndex("DeliveryId");

                    b.ToTable("DeliverySteps");
                });

            modelBuilder.Entity("src.Domain.Entities.Invite", b =>
                {
                    b.Property<int>("InviteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("InviteId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("UserInvitedId")
                        .HasColumnType("int");

                    b.HasKey("InviteId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserInvitedId");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("src.Domain.Entities.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("MessageId"));

                    b.Property<int>("DealId")
                        .HasColumnType("int");

                    b.Property<string>("TextMessage")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("MessageId");

                    b.HasIndex("DealId");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("src.Domain.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Email = "super@user.com",
                            Login = "super",
                            Name = "Super User",
                            Password = "sQYVruLpNUD+dTbZGoDQc4QWpPyuwcA3mv7nYpp6eCA=:4ZRWQMD+f+980pqysc9s+g=="
                        });
                });

            modelBuilder.Entity("src.Domain.Entities.UserLocation", b =>
                {
                    b.Property<int>("UserLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("UserLocationId"));

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<double>("Lat")
                        .HasColumnType("double");

                    b.Property<double>("Lng")
                        .HasColumnType("double");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ZipCode")
                        .HasColumnType("int");

                    b.HasKey("UserLocationId");

                    b.HasIndex("UserId");

                    b.ToTable("UserLocations");

                    b.HasData(
                        new
                        {
                            UserLocationId = 1,
                            Active = false,
                            Address = "123 Main St",
                            City = "São Paulo",
                            Lat = -23.550519999999999,
                            Lng = -46.633308,
                            State = "SP",
                            UserId = 1,
                            ZipCode = 12345
                        });
                });

            modelBuilder.Entity("src.Domain.Entities.Bid", b =>
                {
                    b.HasOne("src.Domain.Entities.Deal", null)
                        .WithMany("Bids")
                        .HasForeignKey("DealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("src.Domain.Entities.User", null)
                        .WithMany("Bids")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("src.Domain.Entities.Deal", b =>
                {
                    b.HasOne("src.Domain.Entities.User", null)
                        .WithMany("Deals")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("src.Domain.Entities.DealImage", b =>
                {
                    b.HasOne("src.Domain.Entities.Deal", null)
                        .WithMany("DealImages")
                        .HasForeignKey("DealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("src.Domain.Entities.DealLocation", b =>
                {
                    b.HasOne("src.Domain.Entities.Deal", null)
                        .WithOne("Location")
                        .HasForeignKey("src.Domain.Entities.DealLocation", "DealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("src.Domain.Entities.Delivery", b =>
                {
                    b.HasOne("src.Domain.Entities.Deal", null)
                        .WithOne("Delivery")
                        .HasForeignKey("src.Domain.Entities.Delivery", "DealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("src.Domain.Entities.User", null)
                        .WithMany("Deliveries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("src.Domain.Entities.DeliverySteps", b =>
                {
                    b.HasOne("src.Domain.Entities.Delivery", "Delivery")
                        .WithMany("Steps")
                        .HasForeignKey("DeliveryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Delivery");
                });

            modelBuilder.Entity("src.Domain.Entities.Invite", b =>
                {
                    b.HasOne("src.Domain.Entities.User", "User")
                        .WithMany("InvitesSent")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("src.Domain.Entities.User", "UserInvited")
                        .WithMany("InvitesReceived")
                        .HasForeignKey("UserInvitedId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("UserInvited");
                });

            modelBuilder.Entity("src.Domain.Entities.Message", b =>
                {
                    b.HasOne("src.Domain.Entities.Deal", null)
                        .WithMany("Messages")
                        .HasForeignKey("DealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("src.Domain.Entities.User", null)
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("src.Domain.Entities.UserLocation", b =>
                {
                    b.HasOne("src.Domain.Entities.User", null)
                        .WithMany("Locations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("src.Domain.Entities.Deal", b =>
                {
                    b.Navigation("Bids");

                    b.Navigation("DealImages");

                    b.Navigation("Delivery");

                    b.Navigation("Location");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("src.Domain.Entities.Delivery", b =>
                {
                    b.Navigation("Steps");
                });

            modelBuilder.Entity("src.Domain.Entities.User", b =>
                {
                    b.Navigation("Bids");

                    b.Navigation("Deals");

                    b.Navigation("Deliveries");

                    b.Navigation("InvitesReceived");

                    b.Navigation("InvitesSent");

                    b.Navigation("Locations");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
