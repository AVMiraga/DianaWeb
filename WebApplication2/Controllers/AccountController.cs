using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Models.ViewModel;

namespace WebApplication2.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
			UserManager<AppUser> userManager, 
			SignInManager<AppUser> signInManager, 
			RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
        }

        public async Task<IActionResult> Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginVm loginVm, string? returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(loginVm);
			}

			AppUser user = await _userManager.FindByNameAsync(loginVm.LoginId);

			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(loginVm.LoginId);
			
				if(user == null)
				{
					ModelState.AddModelError("", "Username Or Password is incorrect.");
					return View(loginVm);
				}
			}

			var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, loginVm.RememberMe, false);

			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Username Or Password is incorrect.");
				return View(loginVm);
			}

			if(result.IsLockedOut)
			{
				return View("AccountLocked");
			}

			if (returnUrl != null && returnUrl != "/Account/Register" && returnUrl != "/Account/Login")
			{
				return Redirect(returnUrl);
			}

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterVm registerVm, string? returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(registerVm);
			}

			AppUser user = new AppUser
			{
				FirstName = registerVm.FirstName,
				LastName = registerVm.LastName,
				UserName = registerVm.Username,
				Email = registerVm.Email
			};

			var result = await _userManager.CreateAsync(user, registerVm.Password);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

				return View(registerVm);
			}

			await _signInManager.SignInAsync(user, isPersistent: false);

			await _userManager.AddToRoleAsync(user, MyRoles.Admin.ToString());

			if(returnUrl  != null && returnUrl != "/Account/Register" && returnUrl != "/Account/Login")
			{
				return Redirect(returnUrl);
			}

			

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> CreateRole()
		{
			foreach (var role in Enum.GetNames(typeof(MyRoles)))
			{
				if (!await _roleManager.RoleExistsAsync(role))
				{
					await _roleManager.CreateAsync(new IdentityRole(role));
				}
			}
			return RedirectToAction("Index", "Home");
		}
	}
}
