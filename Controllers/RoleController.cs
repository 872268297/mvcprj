using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using mvc.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services;
using Newtonsoft.Json.Linq;
using mvc.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace mvc.Controllers
{

    public class RoleController : BaseController
    {
        private readonly MyDbContext _dbcontext;
        private readonly IMemoryCache _memoryCache;

        public RoleController(MyDbContext dbcontext, IMemoryCache memoryCache)
        {
            _dbcontext = dbcontext;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            if (!CheckLogin())
            {
                return Redirect("../Admin/Login");
            }
            return View();
        }

        public IActionResult MenuManager()
        {
            if (!CheckLogin())
            {
                return Redirect("../Admin/Login");
            }
            return View();
        }

        public IActionResult PermissionManager()
        {
            if (!CheckLogin())
            {
                return Redirect("../Admin/Login");
            }
            return View();
        }

        #region 角色管理api

        [HttpPost]
        [Route("api/Role/EditRolePermission")]
        public async Task<IActionResult> EditRolePermission()
        {
            UserData data = UserData.Current;
            if (data == null)
            {
                return NotFound();
            }
            try
            {
                string idstr = Request.Form["roleid"];
                string perstrs = Request.Form["perstrs"];
                int id = int.Parse(idstr);
                string[] perIdsArr = perstrs.Split('|');
                var oldPers = await _dbcontext.RolePermissions.Where(c => c.RoleId == id).ToListAsync();
                _dbcontext.RolePermissions.RemoveRange(oldPers);
                List<RolePermission> newlist = new List<RolePermission>();
                foreach(var item in perIdsArr)
                {
                    newlist.Add(new RolePermission()
                    {
                        PermissionId = int.Parse(item),
                        RoleId = id
                    });
                }
                await _dbcontext.RolePermissions.AddRangeAsync(newlist);
                await _dbcontext.SaveChangesAsync();
                return Json(true, "修改成功");
            }
            catch (Exception ex)
            {
                return Json(false, ex.ToString());
            }
        }

        [HttpPost]
        [Route("api/Role/GetRoleList")]
        public async Task<IActionResult> GetRoleList()
        {
            UserData data = UserData.Current;
            if (data == null)
            {
                return NotFound();
            }
            string keyword = Request.Form["keyword"];
            var query = from c in _dbcontext.Roles select c;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(s => s.RoleName.Contains(keyword));
            }
            List<Role> list = await query.ToListAsync();
            return Json(true, "", list);
        }

        [HttpPost]
        [Route("api/Role/AddRole")]
        public async Task<IActionResult> AddRole(Role item)
        {
            try
            {
                UserData data = UserData.Current;
                if (data == null)
                {
                    return NotFound();
                }
                await _dbcontext.Roles.AddAsync(item);
                await _dbcontext.SaveChangesAsync();
                return Json(true, "添加成功");
            }
            catch (Exception e)
            {
                return Json(false, e.ToString());
            }
        }

        [HttpPost]
        [Route("api/Role/EditRole")]
        public async Task<IActionResult> EditRole(Role item)
        {
            try
            {
                UserData data = UserData.Current;
                if (data == null)
                {
                    return NotFound();
                }
                Role obj = await _dbcontext.Roles.FirstAsync(c => c.Id == item.Id);
                obj.RoleName = item.RoleName;
                await _dbcontext.SaveChangesAsync();
                return Json(true, "修改成功");
            }
            catch (Exception e)
            {
                return Json(false, e.ToString());
            }
        }

        [HttpPost]
        [Route("api/Role/DeleteRole")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                UserData data = UserData.Current;
                if (data == null)
                {
                    return NotFound();
                }

                Role item = new Role() { Id = id };

                _dbcontext.Entry(item).State = EntityState.Deleted;

                bool result = await _dbcontext.SaveChangesAsync() > 0;
                return Json(result, result ? "删除成功" : "删除失败");
            }
            catch (Exception e)
            {
                return Json(false, e.ToString());
            }
        }

        #endregion


        #region 菜单管理api

        private async Task RefleshMenuCache()
        {
            List<MenuItem> menu = await (from c in _dbcontext.MenuItems orderby c.Index select c).ToListAsync();
            _memoryCache.Set("menu_list", menu);
        }

        [HttpPost]
        [Route("api/Role/GetMenuList")]
        public async Task<IActionResult> GetMenuList()
        {
            UserData data = UserData.Current;
            if (data == null)
            {
                return NotFound();
            }
            string keyword = Request.Form["keyword"];
            var query = from c in _dbcontext.MenuItems select c;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(s => s.Name.Contains(keyword));
            }
            query = query.OrderBy(c => c.Index);
            List<MenuItem> list = await query.ToListAsync();
            return Json(true, "", list);
        }

        [HttpPost]
        [Route("api/Role/AddMenu")]
        public async Task<IActionResult> AddMenu(MenuItem item)
        {
            try
            {
                UserData data = UserData.Current;
                if (data == null)
                {
                    return NotFound();
                }
                await _dbcontext.MenuItems.AddAsync(item);
                await _dbcontext.SaveChangesAsync();
                await RefleshMenuCache();
                return Json(true, "添加成功");
            }
            catch (Exception e)
            {
                return Json(false, e.ToString());
            }
        }

        [HttpPost]
        [Route("api/Role/EditMenu")]
        public async Task<IActionResult> EditMenu(MenuItem item)
        {
            try
            {
                UserData data = UserData.Current;
                if (data == null)
                {
                    return NotFound();
                }
                MenuItem obj = await _dbcontext.MenuItems.FirstAsync(c => c.Id == item.Id);
                obj.Index = item.Index;
                obj.Name = item.Name;
                obj.ParentId = item.ParentId;
                obj.RequirePermissionId = item.RequirePermissionId;
                obj.Url = item.Url;
                await _dbcontext.SaveChangesAsync();
                await RefleshMenuCache();
                return Json(true, "修改成功");
            }
            catch (Exception e)
            {
                return Json(false, e.ToString());
            }
        }

        [HttpPost]
        [Route("api/Role/DeleteMenu")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                UserData data = UserData.Current;
                if (data == null)
                {
                    return NotFound();
                }

                if (await (from c in _dbcontext.MenuItems where c.ParentId == id select 1).AnyAsync())
                {
                    return Json(false, "存在子菜单,无法删除");
                }

                MenuItem item = new MenuItem() { Id = id };

                _dbcontext.Entry(item).State = EntityState.Deleted;

                bool result = await _dbcontext.SaveChangesAsync() > 0;
                await RefleshMenuCache();
                return Json(result, result ? "删除成功" : "删除失败");
            }
            catch (Exception e)
            {
                return Json(false, e.ToString());
            }
        }
        #endregion
    }
}