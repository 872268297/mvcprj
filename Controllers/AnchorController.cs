using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
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

        public AnchorController(ILiveClassService liveClass, IUserService _userService, IAnchorService _anchorService)
        {
            _liveClass = liveClass;
            this._userService = _userService;
            this._anchorService = _anchorService;
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


            ViewBag.Dict_Class = JsonConvert.SerializeObject(await _liveClass.GetDict(), settings);

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

            ViewBag.Dict_Class = JsonConvert.SerializeObject(await _liveClass.GetDict(), settings);

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
                Notice = GetVal("Notice")
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
    }
}