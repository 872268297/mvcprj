using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using mvc.Entities;
using mvc.Models;
using Services;

namespace mvc.Controllers
{
    public class ClassController : BaseController
    {

        private readonly ILiveClassService _liveClass;
        private readonly IUserService _userService;

        public ClassController(ILiveClassService liveClass, IUserService _userService)
        {
            _liveClass = liveClass;
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