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
    public class DirectoryController : BaseController
    {

        private readonly ILiveClassService _liveClass;
        private readonly IUserService _userService;
        private readonly IAnchorService _anchorService;

        public DirectoryController(ILiveClassService liveClass
            , IUserService _userService
            , IAnchorService _anchorService
        )
        {
            _liveClass = liveClass;
            this._userService = _userService;
            this._anchorService = _anchorService;
        }

        [Route("Directory/{id?}")]
        public async Task<IActionResult> Index(int id)
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
                ViewBag.User = "";
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            ViewBag.UserAssetJson = Newtonsoft.Json.JsonConvert.SerializeObject(asset, settings);
            ViewBag.UserAsset = asset;

            Dictionary<int, List<LiveClass>> classDict = await _liveClass.GetDict();

            List<BroadcastRoomDTO> list = await _anchorService.GetRoomListLiving(id);

            ViewBag.RoomList = list;

            ViewBag.classDict = classDict;

            return View();
        }
    }
}