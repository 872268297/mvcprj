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
    public class RoomController : BaseController
    {
        private readonly ILiveClassService _liveClass;

        private readonly IUserService _userService;

        private readonly IServerService _serverService;

        private readonly IAnchorService _anchorService;

        public RoomController(ILiveClassService liveClass
            , IUserService _userService
            , IServerService _serverService
            , IAnchorService _anchorService
            )
        {
            _liveClass = liveClass;
            this._userService = _userService;
            this._serverService = _serverService;
            this._anchorService = _anchorService;
        }

        [Route("Room/{id?}")]
        public async Task<IActionResult> Index(int id)
        {
            BroadcastRoomDTO room = await _anchorService.GetRoomByRoomNum(id);
            if (room == null) return NotFound();

            UserData user = UserData.Current;
            UserAsset asset = null;
            if (user != null)
            {
                ViewBag.User = user.UserName;
                asset = await _userService.GetUserAsset(user.UserId);

            }
            else
            {
                ViewBag.User = "";
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            ViewBag.UserAssetJson = Newtonsoft.Json.JsonConvert.SerializeObject(asset, settings);
            ViewBag.UserAsset = asset;

            ViewBag.ID = id;

            //主播头像
            //ViewBag.AnchorPicUrl = "/upload/QQ图片20190402225643.jpg";

            ViewBag.RTMPAddress = _serverService.GetRtmpAddress();

            ViewBag.Room = room;

            Dictionary<int, List<LiveClass>> classDict = await _liveClass.GetDict();

            ViewBag.classDict = classDict;

            return View();
        }
    }
}