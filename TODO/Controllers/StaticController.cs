using Microsoft.AspNetCore.Mvc;

namespace TODO.Controllers
{
    public class StaticController : Controller
    {
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Rules()
        {
            return View();
        }
    }
}
