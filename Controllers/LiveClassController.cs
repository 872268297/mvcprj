using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using mvc.Models;
using Services;

namespace mvc.Controllers
{
    public class LiveClassController : BaseController
    {
        private readonly MyDbContext _dbcontext;
        private readonly IMemoryCache _memoryCache;
        private readonly ILiveClassService _liveClassService;

        public LiveClassController(MyDbContext dbcontext, IMemoryCache memoryCache, ILiveClassService liveClassService)
        {
            _dbcontext = dbcontext;
            _memoryCache = memoryCache;
            _liveClassService = liveClassService;
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
            List<LiveClass> list = await _liveClassService.AllList();

            return Json(true, "成功", list, list.Count);
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

            return Json(await _liveClassService.List(keyword, page, rows));
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

            string Name = GetVal("Name", "");
            string ID = GetVal("ID", "0");
            string Order = GetVal("Order", "0");
            string ParentId = GetVal("ParentId", "0");

            LiveClass c = new LiveClass()
            {
                Id = int.Parse(ID),
                Name = Name,
                Order = int.Parse(Order),
                ParentId = int.Parse(ParentId)
            };

            await _liveClassService.Edit(c);

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

            await _liveClassService.Delete(idList);

            return Json(true, "成功", "操作成功");
        }
    }
}