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
    public class RoomController : BaseController
    {
        private readonly ILiveClassService _liveClass;

        private readonly IUserService _userService;

        public RoomController(ILiveClassService liveClass, IUserService _userService)
        {
            _liveClass = liveClass;
            this._userService = _userService;
        }

        [Route("Room/{id?}")]
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
            ViewBag.UserAssetJson = Newtonsoft.Json.JsonConvert.SerializeObject(asset);
            ViewBag.UserAsset = asset;
            ViewBag.ID = id;

            //主播头像
            ViewBag.AnchorPicUrl = "/upload/QQ图片20190402225643.jpg";

            Dictionary<int, List<LiveClass>> classDict = await _liveClass.GetDict();

            ViewBag.classDict = classDict;

            return View();
        }
    }
}