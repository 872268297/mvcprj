using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using mvc.Entities;
using mvc.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Services;

namespace mvc.Controllers
{
    public class AnchorController : BaseController
    {
        private readonly ILiveClassService _liveClass;

        private readonly IUserService _userService;

        private readonly IAnchorService _anchorService;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly MyDbContext _dbContext;

        public AnchorController(ILiveClassService liveClass
            , IUserService _userService
            , IAnchorService _anchorService
            , IHostingEnvironment _hostingEnvironment
            , MyDbContext _dbContext
            )
        {
            _liveClass = liveClass;
            this._userService = _userService;
            this._anchorService = _anchorService;
            this._hostingEnvironment = _hostingEnvironment;
            this._dbContext = _dbContext;
        }


        public async Task<IActionResult> Index()
        {
            UserData user = UserData.Current;
            UserAsset asset = null;
            if (user != null)
            {
                if (!await _anchorService.IsAnchor(user.UserId))
                {
                    return RedirectToAction("NewRoom");
                }

                ViewBag.User = user.UserName;
                asset = await _userService.GetUserAsset(user.UserId);

            }
            else
            {
                return Redirect("/");
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var liveClass = await _liveClass.GetDict();
            ViewBag.classDict = liveClass;


            ViewBag.Dict_Class = JsonConvert.SerializeObject(liveClass, settings);

            var romm = await _anchorService.GetRoomByUserId(user.UserId);
            ViewBag.RoomInfo = romm;
            ViewBag.UserAssetJson = Newtonsoft.Json.JsonConvert.SerializeObject(asset, settings);
            ViewBag.UserAsset = asset;


            return View();
        }


        public async Task<IActionResult> NewRoom()
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

            var liveClass = await _liveClass.GetDict();
            ViewBag.classDict = liveClass;


            ViewBag.Dict_Class = JsonConvert.SerializeObject(liveClass, settings);

            ViewBag.UserAssetJson = Newtonsoft.Json.JsonConvert.SerializeObject(asset, settings);
            ViewBag.UserAsset = asset;

            return View();
        }


        [Route("api/Anchor/CreateBroadcastRoom")]
        public async Task<IActionResult> CreateBroadcastRoom()
        {
            UserData user = UserData.Current;
            if (user == null)
            {
                return Json(false, "没有登录", null);
            }
            var room = new BroadcastRoom()
            {
                UserId = user.UserId,
                ClassId = int.Parse(GetVal("CLASSID")),
                Name = GetVal("Name"),
                Notice = GetVal("Notice")
            };
            return Json(await _anchorService.CreateBroadcastRoom(user.UserId, room));
        }

        [Route("api/Anchor/SetRoomInfo")]
        public async Task<IActionResult> SetRoomInfo()
        {
            UserData user = UserData.Current;
            if (user == null)
            {
                return Json(false, "没有登录", null);
            }
            var room = new BroadcastRoom()
            {
                UserId = user.UserId,
                ClassId = int.Parse(GetVal("CLASSID")),
                Name = GetVal("Name"),
                Notice = GetVal("Notice"),
                CoverUrl = GetVal("CoverUrl"),
                IsCustomCover = GetVal("IsCustomCover") == "true" ? true : false
            };
            return Json(await _anchorService.SetRoomInfo(user.UserId, room));
        }


        [Route("api/Anchor/UpdateStreamCode")]
        public async Task<IActionResult> UpdateStreamCode()
        {
            UserData user = UserData.Current;
            if (user == null)
            {
                return Json(false, "没有登录", null);
            }

            return Json(await _anchorService.UpdateStreamCode(user.UserId));
        }


        [Route("api/Anchor/StartBroadcast")]
        public async Task<IActionResult> StartBroadcast()
        {
            UserData user = UserData.Current;
            if (user == null)
            {
                return Json(false, "没有登录", null);
            }
            return Json(await _anchorService.StartBroadcast(user.UserId));
        }

        [Route("api/Anchor/StopBroadcast")]
        public async Task<IActionResult> StopBroadcast()
        {
            UserData user = UserData.Current;
            if (user == null)
            {
                return Json(false, "没有登录", null);
            }

            return Json(await _anchorService.StopBroadcast(user.UserId));
        }


        [Route("api/Anchor/UploadCover")]
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
                    var filePath = webRootPath + "/upload/cover/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    string url = "/upload/cover/" + newFileName;

                    return Json(true, "上传成功", url);
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