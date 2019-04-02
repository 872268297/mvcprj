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
    public class ClassController : BaseController
    {

        private readonly ILiveClassService _liveClass;

        public ClassController(ILiveClassService liveClass)
        {
            _liveClass = liveClass;
        }

        public async Task<IActionResult> Index()
        {
            UserData user = UserData.Current;
            if (user != null)
            {
                ViewBag.User = user.UserName;
            }
            else
            {
                ViewBag.User = "";
            }

            Dictionary<int, List<LiveClass>> classDict = await _liveClass.GetDict();

            ViewBag.classDict = classDict;

            return View();
        }
    }
}