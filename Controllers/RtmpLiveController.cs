using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace mvc.Controllers
{
    public class RtmpLiveController : Controller
    {

        private readonly IAnchorService _anchorService;

        public RtmpLiveController(IAnchorService anchorService)
        {
            _anchorService = anchorService;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            //处理推流验证逻辑
            string name = Request.Form["name"].ToString();
            string pass = Request.Form["pass"].ToString();

            if (await _anchorService.CheckStreamCodeValid(name, pass))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }


        }
    }
}