using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using mvc.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services;
using Newtonsoft.Json.Linq;

namespace mvc.Controllers
{

    public class FirstController : ApiController
    {

        private readonly IUserService _userService;

        public FirstController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetData()
        {
            List<object> list = new List<object>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(new
                {
                    Id = i + 1,
                    Name = "hhh"
                });
            }
            return Json(true, "", list);
        }

        [HttpPost]
        public async Task<IActionResult> Reg()
        {
            string r = await _userService.Register(new User()
            {
                UserName = Request.Form["username"],
                Password = Request.Form["password"]
            });
            return Json(r == "true", r, null);
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            return Json(true, "", await _userService.GetList());
        }
    }
}