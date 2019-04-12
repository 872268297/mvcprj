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
        public DbSet<User> Users { get; set; }//�û�
        public DbSet<Role> Roles { get; set; }//��ɫ
        public DbSet<Permission> Permissions { get; set; }//Ȩ��
        public DbSet<RolePermission> RolePermissions { get; set; }//��ɫȨ�޹�����
        public DbSet<UserRole> UserRoles { get; set; }//�û���ɫ������
        public DbSet<UserPermission> UserPermissions { get; set; }//�û�Ȩ�޹�����
        public DbSet<MenuItem> MenuItems { get; set; }//��̨�˵���
        public DbSet<Anchor> Anchors { get; set; }//����
        public DbSet<BroadcastRoom> BroadcastRooms { get; set; }//ֱ����
        public DbSet<UserAsset> UserAssets { get; set; }//�û��Ʋ���Դ
        public DbSet<ComsumeRecord> ComsumeRecords { get; set; }//���Ѽ�¼
        public DbSet<Gift> Gifts { get; set; }//����
        public DbSet<RechargeRecord> RechargeRecords { get; set; }//��ֵ��¼
        public DbSet<GiftReceiveRecord> GiftReceiveRecords { get; set; }//�������ͼ�¼
        public DbSet<UserGift> UserGifts { get; set; }//�û�ӵ������
        public DbSet<Order> Orders { get; set; }//����
        public DbSet<LiveClass> LiveClasses { get; set; }//ֱ������
        public DbSet<Server> Servers { get; set; }//������
        public DbSet<UserFollow> UserFollows { get; set; }//�û���ע
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFollow>().HasKey(t => new { t.UserId, t.AnchorId });

        }
    }
}