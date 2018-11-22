using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace mvc.Controllers
{
    [EnableCors("corsSample")]
    public class BaseController : Controller
    {
        public JsonResult Json(bool success, string message, object data = null)
        {
            return Json(new JsonModel(success, message, data));
        }

        public bool CheckLogin()
        {
            return !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("UserId"));
        }
    }
}