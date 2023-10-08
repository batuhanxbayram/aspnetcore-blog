using Blog.Entity.DTOs.User;
using Blog.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Webui.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class Auth : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public Auth(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
        // Login olabilmek için authorizeı login page için iptal ediyoruz AllowAnonymous ile 
		[HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home", new { Area = "Admin" });
                    }
                    else
                    {
                        ModelState.AddModelError("", "E-posta adresiniz veya şifreniz yanlıştır.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-posta adresiniz veya şifreniz yanlıştır.");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
           
            await _signInManager.SignOutAsync(); 
            return RedirectToAction("Index", "Home",new {Area=""});

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
	        return View();
        }
    }
}
