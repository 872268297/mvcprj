using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _hostingEnvironment;

        public LiveClassController(MyDbContext dbcontext
            , IMemoryCache memoryCache
            , ILiveClassService liveClassService
            , IHostingEnvironment _hostingEnvironment)
        {
            _dbcontext = dbcontext;
            _memoryCache = memoryCache;
            _liveClassService = liveClassService;
            this._hostingEnvironment = _hostingEnvironment;
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
            string ImgUrl = GetVal("ImgUrl", "");

            LiveClass c = new LiveClass()
            {
                Id = int.Parse(ID),
                Name = Name,
                Order = int.Parse(Order),
                ParentId = int.Parse(ParentId),
                ImgUrl = ImgUrl
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


        [Route("api/LiveClass/UploadCover")]
        public async Task<IActionResult> UploadCover()
        {
            var date = Request;
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var formFile = files[0];
            try
            {
                UserData user = UserData.Current;
                if (user == null)
                {
                    throw new Exception("没有登录");
                }

                if (formFile.Length > 0)
                {
                    string fileExt = Path.GetExtension(formFile.FileName);
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string newFileName = System.Guid.NewGuid().ToString() + fileExt; //随机生成新的文件名
                    var filePath = webRootPath + "/upload/ClassCover/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    return Json(true, "上传成功", "/upload/ClassCover/" + newFileName);
                }
            }
            catch (Exception e)
            {
                return Json(false, "上传失败", e.Message);
            }

            return Json(false, "上传失败", "");
        }
    }
}