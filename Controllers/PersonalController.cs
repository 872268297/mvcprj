using System;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace mvc.Controllers
{
    public class PersonalController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly MyDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly ILiveClassService _liveClassService;

        public PersonalController(IHostingEnvironment _hostingEnvironment
            , MyDbContext _dbContext
            , IUserService _userService
            , ILiveClassService _liveClassService
            )
        {
            this._hostingEnvironment = _hostingEnvironment;
            this._dbContext = _dbContext;
            this._userService = _userService;
            this._liveClassService = _liveClassService;
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

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            ViewBag.UserAssetJson = Newtonsoft.Json.JsonConvert.SerializeObject(asset, settings);
            ViewBag.UserAsset = asset;

            Dictionary<int, List<LiveClass>> classDict = await _liveClassService.GetDict();
            ViewBag.classDict = classDict;

            return View();
        }

        public async Task<IActionResult> EditInfo()
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
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            ViewBag.UserAssetJson = Newtonsoft.Json.JsonConvert.SerializeObject(asset, settings);
            ViewBag.UserAsset = asset;

            Dictionary<int, List<LiveClass>> classDict = await _liveClassService.GetDict();
            ViewBag.classDict = classDict;


            return View();
        }

        public async Task<IActionResult> ChangePWD()
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
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            ViewBag.UserAssetJson = Newtonsoft.Json.JsonConvert.SerializeObject(asset, settings);
            ViewBag.UserAsset = asset;

            Dictionary<int, List<LiveClass>> classDict = await _liveClassService.GetDict();
            ViewBag.classDict = classDict;

            return View();
        }

        [Route("api/Personal/ChangePWD")]
        public async Task<IActionResult> ChangePWDApi()
        {
            UserData user = UserData.Current;

            if (user != null)
            {
                var result = await _userService.ChangePWD(user.UserName, GetVal("cur_pwd"), GetVal("new_pwd"));

                return Json(result);
            }
            else
            {
                return Json(false, "未登录", "");
            }


        }

        [Route("api/Personal/EditInfo")]
        public async Task<IActionResult> EditInfoApi()
        {
            UserData user = UserData.Current;

            if (user == null) return NotFound();
            try
            {
                int userid = user.UserId;
                string sex = GetVal("sex", "男");
                int age = int.Parse(GetVal("age", "0"));
                string sign = GetVal("sign", "");
                UserAsset asset = await _userService.GetUserAsset(userid);

                asset.Sex = sex;
                asset.Age = age;
                asset.Sign = sign;
                asset.NickName = GetVal("nickName");
                await _userService.UpdateUserAsset(asset);

            }
            catch (Exception e)
            {
                return Json(false, "修改失败", e.Message);
            }
            return Json(true, "修改成功", null);

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
                    string newFileName = System.Guid.NewGuid().ToString() + fileExt; //随机生成新的文件名
                    var filePath = webRootPath + "/upload/head/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    UserAsset assert = await _userService.GetUserAsset(user.UserId);

                    assert.HeadIcon = "/upload/head/" + newFileName;

                    await _userService.UpdateUserAsset(assert);

                    return Json(true, "上传成功", "/upload/head/" + newFileName);
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