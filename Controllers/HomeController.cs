using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using mvc.Entities;
using mvc.Models;

namespace mvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MyDbContext _myDbContext;

        public HomeController(IMemoryCache memoryCache, MyDbContext myDbContext)
        {
            _memoryCache = memoryCache;
            _myDbContext = myDbContext;
        }

        public async Task<IActionResult> Index()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Login");
            }
            if (!_memoryCache.TryGetValue("menu_list", out List<MenuItem> menu))
            {
                menu = await(from c in _myDbContext.MenuItems orderby c.Index select c).ToListAsync();
                _memoryCache.Set("menu_list", menu);
            }
            var userdata = UserData.Current;
            var pers = userdata.Permissions;
            var perids = from c in pers select c.Id;
            var linq = from c in menu where c.RequirePermissionId == 0 || perids.Contains(c.RequirePermissionId) select c;
            ViewBag.userdata = userdata;
            ViewBag.Menus = linq.ToArray();
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
