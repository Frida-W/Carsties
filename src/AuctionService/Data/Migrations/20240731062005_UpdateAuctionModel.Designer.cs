﻿// <auto-generated />
using System;
using AuctionService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuctionService.Data.Migrations
{
    [DbContext(typeof(AuctionDbContext))]
    [Migration("20240731062005_UpdateAuctionModel")]
    partial class UpdateAuctionModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AuctionService.Entities.Auction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("AuctionEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("CurrentHighBid")
                        .HasColumnType("integer");

                    b.Property<int>("ReservePrice")
                        .HasColumnType("integer");

                    b.Property<string>("Seller")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("SoldAmount")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Winner")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("AuctionService.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuctionId")
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Mileage")
                        .HasColumnType("integer");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AuctionId")
                        .IsUnique();

                    b.ToTable("Items");
                });

            modelBuilder.Entity("AuctionService.Item", b =>
                {
                    b.HasOne("AuctionService.Entities.Auction", "Auction")
                        .WithOne("Item")
                        .HasForeignKey("AuctionService.Item", "AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auction");
                });

            modelBuilder.Entity("AuctionService.Entities.Auction", b =>
                {
                    b.Navigation("Item")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
