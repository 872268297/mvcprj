using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Entities;

namespace Entities
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }//用户
        public DbSet<Role> Roles { get; set; }//角色
        public DbSet<Permission> Permissions { get; set; }//权限
        public DbSet<RolePermission> RolePermissions { get; set; }//角色权限关联表
        public DbSet<UserRole> UserRoles { get; set; }//用户角色关联表
        public DbSet<UserPermission> UserPermissions { get; set; }//用户权限关联表
        public DbSet<MenuItem> MenuItems { get; set; }//后台菜单项
        public DbSet<Anchor> Anchors { get; set; }//主播
        public DbSet<BroadcastRoom> BroadcastRooms { get; set; }//直播间
        public DbSet<UserAsset> UserAssets { get; set; }//用户财产资源
        public DbSet<ComsumeRecord> ComsumeRecords { get; set; }//消费记录
        public DbSet<Gift> Gifts { get; set; }//礼物
        public DbSet<RechargeRecord> RechargeRecords { get; set; }//充值记录
        public DbSet<GiftReceiveRecord> GiftReceiveRecords { get; set; }//礼物赠送记录
        public DbSet<UserGift> UserGifts { get; set; }//用户拥有礼物
        public DbSet<Order> Orders { get; set; }//订单
        public DbSet<LiveClass> LiveClasses { get; set; }//直播分类
        public DbSet<Server> Servers { get; set; }//服务器
        public DbSet<UserFollow> UserFollows { get; set; }//用户关注
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFollow>().HasKey(t => new { t.UserId, t.AnchorId });

        }
    }
}