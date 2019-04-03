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
    public class SearchController : BaseController
    {

        private readonly ILiveClassService _liveClass;

        public SearchController(ILiveClassService liveClass)
        {
            _liveClass = liveClass;
        }
        [Route("Search/{id?}")]
        public async Task<IActionResult> Index(string id)
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