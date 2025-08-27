using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TODO.Models.User;
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
        public IActionResult Register() => View(new CreateUserModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CreateUserModel model)
        {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var (success, errors) = await _accountService.Register(model);

            if (success)
                return RedirectToAction("Index", "Task");

            foreach (var error in errors) {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _accountService.Login(username, password);
            if (result)
                return RedirectToAction("Index", "task");

            return BadRequest("Wrong username or password");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "task");
        }
    }
}
