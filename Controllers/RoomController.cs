using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using Services;

namespace mvc.Controllers
{
    public class RoomController : BaseController
    {
        private readonly ILiveClassService _liveClass;

        public RoomController(ILiveClassService liveClass)
        {
            _liveClass = liveClass;
        }

        [Route("Room/{id?}")]
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.ID = id;
            UserData user = UserData.Current;
            if (user != null)
            {
                ViewBag.User = user.UserName;
            }
            else
            {
                ViewBag.User = "";
            }

            List<LiveClass> classList = await _liveClass.AllList();

            ViewBag.ClassList = classList;

            return View();
        }
    }
}