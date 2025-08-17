using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TODO.Services;

namespace TODO.Controllers
{
    public class AccountController : Controller
    {

        private readonly AccountService _accountService;
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var result = await _accountService.Register(username, password);

            if (result)
                return RedirectToAction("Index", "task");


            
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _accountService.Login(username, password);
            if (result)
                return RedirectToAction("Index", "task");

            return BadRequest("Wrong username or password");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "task");
        }
    }
}
