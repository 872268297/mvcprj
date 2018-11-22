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
using mvc.Util;
using Microsoft.AspNetCore.Http;

namespace mvc.Controllers
{

    public class UserController : ApiController
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

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
        public IActionResult VerificationCode()
        {
            string num = VerificationCodeImage.RandomNum();
            HttpContext.Session.SetString("verificationCode", num);
            return File(VerificationCodeImage.CreateImage(num), "image/png");
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {
            string sessionCode = HttpContext.Session.GetString("verificationCode");
            if (string.IsNullOrWhiteSpace(sessionCode))
            {
                HttpContext.Session.Remove("verificationCode");
                return Json(false, "验证码过期,请刷新验证码");
            }
            string code = Request.Form["code"];
            if (string.IsNullOrWhiteSpace(sessionCode) || code != sessionCode)
            {
                HttpContext.Session.Remove("verificationCode");
                return Json(false, "验证码错误");
            }
            HttpContext.Session.Remove("verificationCode");
            string r = await _userService.Login(Request.Form["username"], Request.Form["password"]);
            return Json(r == "true", r, UserData.Current);
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            UserData user = UserData.Current;
            if (user == null)
            {
                return Json(false, "未登录");
            }
            return Json(true, "", await _userService.GetList());
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            return Json(_userService.LogOut(), "");
        }
    }
}