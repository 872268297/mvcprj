using Microsoft.AspNetCore.Mvc;
using mvc.Entities;
using mvc.Models;
using Services;
using System.Threading.Tasks;

namespace mvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;

        public HomeController(IUserService _userService)
        {
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

            return View();
        }
    }
}