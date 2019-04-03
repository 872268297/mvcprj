using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using Services;
using mvc.Entities;

namespace mvc.Controllers
{
    public class SearchController : BaseController
    {

        private readonly ILiveClassService _liveClass;

        private readonly IUserService _userService;

        public SearchController(ILiveClassService liveClass, IUserService _userService)
        {
            _liveClass = liveClass;
            this._userService = _userService;
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
            ViewBag.UserAssetJson = Newtonsoft.Json.JsonConvert.SerializeObject(asset);
            ViewBag.UserAsset = asset;

            Dictionary<int, List<LiveClass>> classDict = await _liveClass.GetDict();

            ViewBag.classDict = classDict;

            return View();
        }
    }
}