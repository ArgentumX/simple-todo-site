using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TODO.Models.User.view;

namespace TODO.Services
{
    public class AccountService
    {
        private readonly UserManager<MongoUser> _userManager;
        private readonly SignInManager<MongoUser> _signInManager;

        public AccountService(UserManager<MongoUser> userManager, SignInManager<MongoUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool Success, IEnumerable<string> Errors)> Register(RegistrationViewModel model)
        {
            var user = new MongoUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return (true, new List<string>());
            }

            var errors = result.Errors.Select(e => e.Description);
            return (false, errors);
        }

        public async Task<bool> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);
            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
