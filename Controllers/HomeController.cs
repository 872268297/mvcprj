using Entities;
using Microsoft.AspNetCore.Mvc;
using mvc.Entities;
using mvc.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;

        private readonly ILiveClassService _liveClass;

        public HomeController(IUserService _userService, ILiveClassService _liveClass)
        {
            this._userService = _userService;
            this._liveClass = _liveClass;
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

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            ViewBag.UserAssetJson = Newtonsoft.Json.JsonConvert.SerializeObject(asset, settings);
            ViewBag.UserAsset = asset;

            Dictionary<int, List<LiveClass>> classDict = await _liveClass.GetDict();

            ViewBag.classDict = classDict;

            return View();
        }
    }
}