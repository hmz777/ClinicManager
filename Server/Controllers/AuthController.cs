using ClinicProject.Server.Data.DBModels.AppUsers;
using ClinicProject.Server.Helpers;
using ClinicProject.Server.Models.Auth.Login;
using ClinicProject.Server.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ClinicProject.Server.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.stringLocalizer = stringLocalizer;
        }

        [Route("/login")]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                TempData.Put<string>("returnUrl", returnUrl);
            }

            return View();
        }

        [Route("/login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var user = await userManager.FindByNameAsync(loginModel.Username);

            if (user == null)
            {
                TempData.Put("Message", new StatusMessage
                {
                    MessageStatus = MessageStatus.Error,
                    Message = stringLocalizer["UserNotFound"].Value
                });

                return View(loginModel);
            }

            var loginResult = await signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, isPersistent: true, lockoutOnFailure: true);

            if (loginResult.Succeeded)
            {
                var rUrl = returnUrl == null ? TempData.Get<string>("returnUrl") ?? "" : returnUrl;

                if (!string.IsNullOrWhiteSpace(rUrl) && Url.IsLocalUrl(rUrl))
                {
                    return LocalRedirect(rUrl);
                }
                else
                {
                    return NotFound(new StatusMessage { MessageStatus = MessageStatus.Success, Message = stringLocalizer["LoginSuccessNoValidReturnUrl"].Value });
                }
            }
            else
            {
                TempData.Put("Message", new StatusMessage
                {
                    MessageStatus = MessageStatus.Error,
                    Message = stringLocalizer["LoginInvalid"].Value
                });

                return RedirectToAction("Login", "Auth");
            }
        }

        [Authorize]
        [Route("/logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await signInManager.SignOutAsync();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return NotFound(new StatusMessage
            {
                MessageStatus = MessageStatus.Error,
                Message = stringLocalizer["LogoutSuccessfulNoValidReturnUrl"].Value
            });
        }
    }
}
