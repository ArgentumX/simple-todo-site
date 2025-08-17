using Microsoft.AspNetCore.Mvc;

namespace TODO.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Rules()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
