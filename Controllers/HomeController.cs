using Microsoft.AspNetCore.Mvc;

namespace mvc.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}