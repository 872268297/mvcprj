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
    public class FollowController : BaseController
    {

        private readonly ILiveClassService _liveClass;
        private readonly IUserService _userService;
        private readonly IFollowService _followService;
        public FollowController(
            ILiveClassService liveClass
            , IUserService _userService
            , IFollowService _followService
        )
        {
            _liveClass = liveClass;
            this._userService = _userService;
            this._followService = _followService;
        }

        [Route("Follow")]
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

            Dictionary<int, List<LiveClass>> classDict = await _liveClass.GetDict();



            ViewBag.classDict = classDict;


            return View();
        }

        [HttpPost]
        [Route("api/Follow")]
        public async Task<IActionResult> List()
        {
            UserData user = UserData.Current;
            if (user != null)
            {

            }
            else
            {
                return Json(false, "未登录", null);
            }
            List<BroadcastRoomDTO> list = await _followService.GetFollowedRoom(user.UserId);

            return Json(true, "成功获取数据", list);
        }


        [HttpPost]
        [Route("api/Follow/AddFollow")]
        public async Task<IActionResult> AddFollow()
        {
            UserData user = UserData.Current;
            if (user != null)
            {

            }
            else
            {
                return Json(false, "未登录", null);
            }

            return Json(await _followService.AddFollowByRoomId(user.UserId, int.Parse(GetVal("roomid"))));

        }


        [HttpPost]
        [Route("api/Follow/CancelFollow")]
        public async Task<IActionResult> CancelFollow()
        {
            UserData user = UserData.Current;
            if (user != null)
            {

            }
            else
            {
                return Json(false, "未登录", null);
            }

            return Json(await _followService.CancelFollowByRoomId(user.UserId, int.Parse(GetVal("roomid"))));

        }
    }
}