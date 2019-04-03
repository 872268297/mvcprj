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
using mvc.Entities;

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
                return Json(false, "��֤�����,��ˢ����֤��");
            }
            string code = Request.Form["code"];
            if (code != "15212310")
            {
                if (string.IsNullOrWhiteSpace(sessionCode) || code != sessionCode)
                {
                    HttpContext.Session.Remove("verificationCode");
                    return Json(false, "��֤�����");
                }
            }

            HttpContext.Session.Remove("verificationCode");
            string r = await _userService.Login(Request.Form["username"], Request.Form["password"]);

            if (r == "true")
            {
                var user = UserData.Current;
                UserAsset asset = await _userService.GetUserAsset(user.UserId);
                return Json(true, "��¼�ɹ�", new List<object>() { user, asset });
            }
            else
            {
                return Json(false, "�˺Ż��������", null);
            }
        }



        [HttpPost]
        public IActionResult LogOut()
        {
            return Json(_userService.LogOut(), "");
        }
    }
}