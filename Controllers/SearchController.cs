using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using Services;
using mvc.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace mvc.Controllers
{
    public class SearchController : BaseController
    {

        private readonly ILiveClassService _liveClass;

        private readonly IUserService _userService;

        private readonly IAnchorService _anchorService;

        public SearchController(ILiveClassService liveClass, IUserService _userService, IAnchorService _anchorService)
        {
            _liveClass = liveClass;
            this._userService = _userService;
            this._anchorService = _anchorService;
        }

        [Route("Search/{id?}")]
        public async Task<IActionResult> Index(string id)
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

            List<BroadcastRoomDTO> list = await _anchorService.GetRoomList(id);

            ViewBag.ListJson = JsonConvert.SerializeObject(list, settings);

            ViewBag.classDict = classDict;

            ViewBag.Keyword = id;


            return View();
        }
    }
}