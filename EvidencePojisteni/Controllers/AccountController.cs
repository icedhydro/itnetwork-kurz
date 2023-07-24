using EvidencePojisteni.Data;
using EvidencePojisteni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EvidencePojisteni.Controllers
{

	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly ApplicationDbContext context;

		public AccountController(UserManager<IdentityUser> userManager,
								SignInManager<IdentityUser> signInManager,
								ApplicationDbContext context)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.context = context;
		}

		private IActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction(nameof(HomeController.Index), "Home");
			}
		}

		[HttpGet]
		public IActionResult Login(string returnUrl = null, string message = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			ViewData["Message"] = message;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model, string navratovaURL = null)
		{
			ViewData["ReturnUrl"] = navratovaURL;
			if (ModelState.IsValid)
			{
				var vysledekOvereni = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
				if (vysledekOvereni.Succeeded)
				{
					var successMessage = "Přihlášení proběhlo úspěšně.";
					TempData["Message"] = successMessage;
					TempData["AlertType"] = "success";
					return RedirectToLocal(navratovaURL);
				}
				else
				{
					var errorMessage = "Neplatné přihlašovací údaje.";
					TempData["Message"] = errorMessage;
					TempData["AlertType"] = "danger";
					ModelState.AddModelError(string.Empty, "Neplatné přihlašovací údaje.");
					return View(model);
				}
			}

			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();

			TempData["Message"] = "Odhlášení proběhlo úspěšně.";
			TempData["AlertType"] = "info";

			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		[HttpGet]
		public IActionResult Register(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			if (ModelState.IsValid)
			{
				var user = new IdentityUser { UserName = model.Email, Email = model.Email };
				var result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{

					var insured = new Insured
					{
						Id = 0,
						FirstName = model.FirstName,
						LastName = model.LastName,
						Email = model.Email,
						Phone = model.Phone,
						Street = model.Street,
						City = model.City,
						PostalCode = model.PostalCode,
						Insurance = null
					};

					context.Add(insured);
					await context.SaveChangesAsync();

					await signInManager.SignInAsync(user, isPersistent: false);

					TempData["Message"] = "Registrace proběhla úspěšně.";
					TempData["AlertType"] = "success";

					return RedirectToLocal(returnUrl);
				}
				else
				{

					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}

					TempData["Message"] = "Při registraci došlo k chybě.";
					TempData["AlertType"] = "danger";
				}

			}

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> ChangePassword()
		{
			var user = await userManager.GetUserAsync(User) ??
				throw new ApplicationException($"Nepodařilo se načíst uživatele {userManager.GetUserId(User)}.");

			var model = new ChangePasswordViewModel();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = await userManager.GetUserAsync(User) ??
				throw new ApplicationException($"Nepodařilo se načíst uživatele {userManager.GetUserId(User)}.");

			var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

			if (!changePasswordResult.Succeeded)
			{
				return View(model);
			}

			await signInManager.SignInAsync(user, isPersistent: false);

			return RedirectToAction("Index", "Home");
		}

	}
}


