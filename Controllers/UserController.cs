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

            string code = Request.Form["code"];
            if (code != "15212310")
            {
                string sessionCode = HttpContext.Session.GetString("verificationCode");
                if (string.IsNullOrWhiteSpace(sessionCode))
                {
                    HttpContext.Session.Remove("verificationCode");
                    return Json(false, "��֤�����,��ˢ����֤��");
                }

                if (string.IsNullOrWhiteSpace(sessionCode) || code != sessionCode)
                {
                    HttpContext.Session.Remove("verificationCode");
                    return Json(false, "��֤�����");
                }
            }
            else
            {
                string s = HttpContext.Session.GetString("_register");
                if (s != null && s == "_register")
                {
                    HttpContext.Session.Remove("_register");
                }
                else
                {
                    return Json(false, "��֤�����");
                }
            }

            HttpContext.Session.Remove("verificationCode");
            string r = await _userService.Login(GetVal("username"), GetVal("password"));

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
        public async Task<IActionResult> GetUserAsset()
        {
            UserData user = UserData.Current;
            UserAsset asset = null;
            if (user != null)
            {
                asset = await _userService.GetUserAsset(user.UserId);
            }
            else
            {
                return NotFound();
            }
            return Json(true, "�ɹ�", asset);
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            return Json(_userService.LogOut(), "");
        }

        [HttpPost]
        public IActionResult GetDailyTask()
        {
            List<DailyTask> list = new List<DailyTask>() {
                new DailyTask(){
                    Discribe="�ۿ�1��ֱ��",
                    CurrentCount =1 ,
                    TotalCount = 1,
                    Exp = 20,
                    Id= 1,
                    IsComplete = true
                },
                new DailyTask(){
                    Discribe="��ע1������",
                    CurrentCount =1 ,
                    TotalCount = 1,
                    Exp = 50,
                    Id=2,
                    IsComplete = false
                },
                new DailyTask(){
                    Discribe="����1�ε�Ļ",
                    CurrentCount = 0 ,
                    TotalCount = 1,
                    Exp = 100,
                    Id=3,
                    IsComplete = false
                }
            };

            return Json(true, "�ɹ�", list);
        }

        public async Task<IActionResult> CompleteTask()
        {
            UserData user = UserData.Current;
            if (user != null)
            {
                var a = await _userService.GetUserAsset(user.UserId);
                if (a == null)
                {
                    return Json(false, "�û�������", null);
                }
                //��ȡ�Ӿ���
                a.Exp += 50;
                while (a.Exp >= a.Level * 100)
                {
                    a.Exp -= a.Level * 100;
                    a.Level += 1;
                }
                await _userService.UpdateUserAsset(a);
                return Json(true, "��ȡ�ɹ�", null);
            }
            else
            {
                return Json(false, "û�е�¼", null);
            }

        }
    }
}