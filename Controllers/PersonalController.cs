﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using System.IO;
using Services;
using mvc.Entities;

namespace mvc.Controllers
{
    public class PersonalController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly MyDbContext _dbContext;
        private readonly IUserService _userService;
        public PersonalController(IHostingEnvironment _hostingEnvironment, MyDbContext _dbContext, IUserService _userService)
        {
            this._hostingEnvironment = _hostingEnvironment;
            this._dbContext = _dbContext;
            this._userService = _userService;
        }

        public async Task<IActionResult> Index()
        {
            UserData user = UserData.Current;
            UserAsset asset = null;
            if (user != null)
            {
                ViewBag.User = user.UserName;
                asset = await _userService.GetUserAsset(user.UserId);

            }
            else
            {
                return Redirect("/");
            }
            ViewBag.UserAssetJson = Newtonsoft.Json.JsonConvert.SerializeObject(asset);
            ViewBag.UserAsset = asset;
            return View();
        }
        [Route("api/Personal/UploadHead")]
        public async Task<IActionResult> UploadHead()
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
                    string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                    var filePath = webRootPath + "/upload/head" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    UserAsset assert = await _userService.GetUserAsset(user.UserId);

                    assert.HeadIcon = "/upload/head" + newFileName;

                    await _userService.UpdateUserAsset(assert);

                    return Json(true, "上传成功", "/upload/head" + newFileName);
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