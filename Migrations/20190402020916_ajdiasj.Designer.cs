﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace mvc.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20190402020916_ajdiasj")]
    partial class ajdiasj
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Entities.Anchor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Follower");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Anchors");
                });

            modelBuilder.Entity("Entities.ComsumeRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Count");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("GoodGuid");

                    b.Property<decimal>("TotalPrice");

                    b.Property<string>("Type");

                    b.Property<decimal>("UnitPrice");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("ComsumeRecords");
                });

            modelBuilder.Entity("Entities.Gift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Guid");

                    b.Property<string>("ImgUrl");

                    b.Property<bool>("IsShow");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.Property<string>("Type")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Gifts");
                });

            modelBuilder.Entity("Entities.GiftReceiveRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Count");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("GiftId");

                    b.Property<int>("ReceiveUserId");

                    b.Property<int>("SendUserId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("GiftReceiveRecords");
                });

            modelBuilder.Entity("Entities.LiveClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<int>("ParentId");

                    b.HasKey("Id");

                    b.ToTable("LiveClasses");
                });

            modelBuilder.Entity("Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CompleteTime");

                    b.Property<int>("Count");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("GoodGuid")
                        .IsRequired();

                    b.Property<string>("PayWay");

                    b.Property<decimal>("RecevieMoney");

                    b.Property<int>("Status");

                    b.Property<decimal>("ToTalPrice");

                    b.Property<string>("Type");

                    b.Property<decimal>("UnitPrice");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ParentPermissionId");

                    b.Property<string>("PermissionDisplayName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("PermissionName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Entities.RechargeRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("BZ");

                    b.Property<decimal>("Count");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("PayWay");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("RechargeRecords");
                });

            modelBuilder.Entity("Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Entities.RolePermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PermissionId");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("Entities.Server", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("IpAddress");

                    b.Property<int>("Port");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Entities.UserGift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Count");

                    b.Property<int>("GiftId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserGifts");
                });

            modelBuilder.Entity("Entities.UserPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PermissionId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("mvc.Entities.BroadcastRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnchorId");

                    b.Property<bool>("IsLiving");

                    b.Property<DateTime?>("LastLiveTime");

                    b.Property<string>("Name")
                        .HasMaxLength(32);

                    b.Property<int>("RoomNum");

                    b.Property<string>("StreamChannel");

                    b.Property<int>("UserId");

                    b.Property<int>("Viewer");

                    b.HasKey("Id");

                    b.ToTable("BroadcastRooms");
                });

            modelBuilder.Entity("mvc.Entities.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Index");

                    b.Property<string>("Name");

                    b.Property<int>("ParentId");

                    b.Property<int>("RequirePermissionId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("MenuItems");
                });

            modelBuilder.Entity("mvc.Entities.UserAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Exp");

                    b.Property<decimal>("Gold");

                    b.Property<int>("Level");

                    b.Property<decimal>("RechargeAmount");

                    b.Property<decimal>("Silver");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserAssets");
                });
#pragma warning restore 612, 618
        }
    }
}
