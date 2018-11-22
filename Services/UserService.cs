using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _dbContext;
        public UserService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> Register(User user)
        {
            string userName = user.UserName;
            if (userName.Length < 5 || user.Password.Length < 5)
            {
                return "用户名或密码长度不足";
            }
            var linq = from c in _dbContext.Users
                       where c.UserName == userName
                       select 1;

            if (await linq.AnyAsync())
            {
                return "用户名已存在"; //重复名
            }
            user.Password = Md5Util.Encode(Md5Util.Encode(user.Password + "sdf!!~@90sjd") + userName);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return "true";
        }

        public async Task<List<User>> GetList()
        {
            var linq = from c in _dbContext.Users select c;
            return await linq.ToListAsync();
        }

        public async Task<string> Login(string username, string password)
        {
            string md5pwd = Md5Util.Encode(Md5Util.Encode(password + "sdf!!~@90sjd") + username);
            var linq = from c in _dbContext.Users
                       where c.UserName == username && c.Password == md5pwd
                       select c;
            User user = await linq.FirstOrDefaultAsync();
            if (user == null)
            {
                return "用户名或密码不正确";
            }
            UserData userdata = new UserData
            {
                UserId = user.Id,
                UserName = user.UserName,
                LoginTime = DateTime.Now
            };
            try
            {
                var roles = await (from c in _dbContext.UserRoles
                                   join cc in _dbContext.Roles on c.UserId equals cc.Id
                                   where c.UserId == user.Id
                                   select cc).ToListAsync();
                userdata.Roles = roles;
                //角色权限
                var per1 = from c in _dbContext.UserPermissions
                           join cc in _dbContext.Permissions
                           on c.PermissionId equals cc.Id
                           where c.UserId == user.Id
                           select cc;
                //用户权限
                var per2 = from c in _dbContext.UserRoles
                           join cc in _dbContext.RolePermissions on c.RoleId equals cc.RoleId
                           join ccc in _dbContext.Permissions on cc.PermissionId equals ccc.Id
                           where c.UserId == user.Id
                           select ccc;
                var pers = await per1.Union(per2).ToListAsync();
                userdata.Permissions = pers;
                MyHttpContext.Current.Session.Set("UserData", userdata);
                MyHttpContext.Current.Session.SetString("UserId", user.Id.ToString());
                return "true";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public bool LogOut()
        {
            try
            {
                ISession s = MyHttpContext.Current.Session;
                s.Remove("UserData");
                s.Remove("UserId");
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}