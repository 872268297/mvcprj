using Microsoft.AspNetCore.Mvc;
using mvc.Models;

namespace mvc.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
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

            return View();
        }
    }
}