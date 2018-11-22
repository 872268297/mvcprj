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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
    }
}