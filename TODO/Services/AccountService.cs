using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<bool> Register(string username, string password)
        {
            var user = new MongoUser { UserName = username };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
                await _signInManager.SignInAsync(user, isPersistent: true);
            return result.Succeeded;
        }

        public async Task<bool> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, true, false);
            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
