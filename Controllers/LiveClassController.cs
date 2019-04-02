using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using mvc.Models;

namespace mvc.Controllers
{
    public class LiveClassController : BaseController
    {
        private readonly MyDbContext _dbcontext;
        private readonly IMemoryCache _memoryCache;

        public LiveClassController(MyDbContext dbcontext, IMemoryCache memoryCache)
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
            UserData user = UserData.Current;
            if (!user.CheckPermission("直播分类管理"))
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        [Route("api/LiveClass/AllList")]
        public async Task<IActionResult> AllList()
        {

            var query = from c in _dbcontext.LiveClasses select c;

            int count = await (from c in _dbcontext.LiveClasses select 1).CountAsync();
            List<LiveClass> list = await query.ToListAsync();
            return Json(true, "成功", list, count);
        }

        [HttpPost]
        [Route("api/LiveClass/List")]
        public async Task<IActionResult> List()
        {
            UserData data = UserData.Current;
            if (data == null || !data.CheckPermission("直播分类管理"))
            {
                return NotFound();
            }
            string keyword = GetVal("keyword", "");
            int page = int.Parse(GetVal("page", "1"));
            int rows = int.Parse(GetVal("rows", "10"));

            var query = from c in _dbcontext.LiveClasses select c;
            if (keyword != "") query = query.Where(t => t.Name.Contains(keyword));
            int count = await (from c in _dbcontext.LiveClasses select 1).CountAsync();
            List<LiveClass> list = await query.Skip((page - 1) * rows).Take(rows).ToListAsync();
            return Json(true, "成功", list, count);
        }


        [HttpPost]
        [Route("api/LiveClass/Edit")]
        public async Task<IActionResult> Edit()
        {
            UserData data = UserData.Current;
            if (data == null || !data.CheckPermission("直播分类管理"))
            {
                return NotFound();
            }

            string Name = Request.Form["Name"];
            string ID = Request.Form["ID"];
            string Order = Request.Form["Order"];
            string ParentId = Request.Form["ParentId"];

            if (ID == "")
            {
                LiveClass c = new LiveClass()
                {
                    Name = Name,
                    Order = int.Parse(Order),
                    ParentId = int.Parse(ParentId)
                };
                await _dbcontext.LiveClasses.AddAsync(c);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                int id = int.Parse(ID);
                LiveClass c = await _dbcontext.LiveClasses.FirstOrDefaultAsync(t => t.Id == id);
                if (c != null)
                {
                    c.Name = Name;
                    c.Order = int.Parse(Order);
                    c.ParentId = int.Parse(ParentId);
                    _dbcontext.LiveClasses.Update(c);
                    await _dbcontext.SaveChangesAsync();
                }
            }

            return Json(true, "成功", "操作成功");
        }


        [HttpPost]
        [Route("api/LiveClass/Delete")]
        public async Task<IActionResult> Delete()
        {
            UserData data = UserData.Current;
            if (data == null || !data.CheckPermission("直播分类管理"))
            {
                return NotFound();
            }

            string idList = Request.Form["idList"];

            string[] strs = idList.Split(',');
            List<int> lst = new List<int>();
            foreach (string item in strs)
            {
                if (int.TryParse(item, out int i))
                {
                    lst.Add(i);
                }
            }
            var query = from c in _dbcontext.LiveClasses where lst.Contains(c.Id) select c;

            _dbcontext.LiveClasses.RemoveRange(query);

            await _dbcontext.SaveChangesAsync();

            return Json(true, "成功", "操作成功");
        }
    }
}