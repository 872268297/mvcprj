using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace mvc.Controllers
{
    public class RtmpLiveController : Controller
    {
        [HttpPost]
        public IActionResult Index()
        {
            //处理推流验证逻辑
            string name = Request.Form["name"].ToString();
            string pass = Request.Form["pass"].ToString();
            Console.WriteLine("name: {0}\npass:{1}\n", name, pass);
            //_logger.LogInformation("name: {0}\npass:{1}\n", name, pass);
            return Ok();
            //return Content("123");
        }
    }
}