using Cara.Business.DTOs;
using Cara.Business.Interfaces;
using Cara.Core.Entities;
using Cara.WebUI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Cara.WebUI.Utilities.Helper;

namespace Cara.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly IMailService _mailService;
		public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
			RoleManager<IdentityRole> roleManager, IMailService mailService, IWebHostEnvironment env)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_mailService = mailService;
			_env = env;
		}

		public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid) return View(registerViewModel);

            AppUser appUser = new()
            {
                Fullname= registerViewModel.Fullname,
                UserName= registerViewModel.Username,
                Email= registerViewModel.Email, 
                IsActive = true
            };

            var identityResult = await _userManager.CreateAsync(appUser, registerViewModel.Password);
            if(!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(registerViewModel);
            }

            await _userManager.AddToRoleAsync(appUser, RoleType.Member.ToString());

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginViewModel.UsernameOrEmail);
                if (user == null) 
                {
                    ModelState.AddModelError("", "Username/Email or password incorrect");
                    return View(loginViewModel);
                }
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, true);

            if(signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Login timeout expired");
                return View(loginViewModel);    
            }
            if(!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username/Email or password incorrect");
                return View(loginViewModel);
            }
            if(!user.IsActive)
            {
                ModelState.AddModelError("", "User is not found");
                return View(loginViewModel);    
            }

             return RedirectToAction("Index", "Home"); 
        }

        public async Task<IActionResult> Logout()
        {
            if(User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            return BadRequest();
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!ModelState.IsValid) return View(forgotPasswordViewModel);

            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if (user is null)
            {
                ModelState.AddModelError("Email", $"User not found by email: {forgotPasswordViewModel.Email}");
                return View(forgotPasswordViewModel);
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string? link = Url.Action("ResetPassword", "Auth", new {userId = user.Id, token = token},HttpContext.Request.Scheme);

			var userName = await _userManager.FindByIdAsync(user.Id);
            string fullName = $"{userName.Fullname}";
			string body = await GetEmailTemplateAsync(link, fullName);

			MailRequestDto mailRequestDto = new()
			{
				ToEmail = user.Email,
				Subject = "Reset your password",
				Body = body
			};

			await _mailService.SendEmailAsync(mailRequestDto);

			return RedirectToAction(nameof(Login));
		}

        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            if(string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest(); 

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound();

			ViewBag.UserName = user.UserName;
            

			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel, string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest();
            if(!ModelState.IsValid) return View(resetPasswordViewModel);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound();

           var identityResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordViewModel.NewPassword);
            if(!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach (var role in Enum.GetValues(typeof(RoleType)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                  await _roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }

            return Json("Roles Created!");
        }

		public async Task<string> GetEmailTemplateAsync(string link, string fullName)
		{
			string folder = @"admin\assets\database\templates";
			string path = Path.Combine(_env.WebRootPath, folder + "\\", "ConfirmEmail.html");

			using StreamReader streamReader = new StreamReader(path);
			string result = await streamReader.ReadToEndAsync();
			result = result.Replace("[reset_password_url]", link);
			result = result.Replace("[user_full_name]", fullName);
			streamReader.Close();
			return result;
		}
	}
}
