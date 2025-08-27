using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TODO.Models.User.view;
using TODO.Services;

namespace TODO.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly AccountService _accountService;
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View(new RegistrationViewModel());

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
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
        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _accountService.Login(model);
            if (result)
                return RedirectToAction("Index", "task");


            ModelState.AddModelError("INVALID_LOGIN_ATTEMPT", "Wrong username or password");
            return View(model);
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
