using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;

namespace mvc.Controllers
{
    public class RoomController : BaseController
    {
        [Route("Room/{id?}")]
        public IActionResult Index(int id)
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
            
            return View();
        }
    }
}