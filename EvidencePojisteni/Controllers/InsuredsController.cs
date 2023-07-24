using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EvidencePojisteni.Data;
using EvidencePojisteni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EvidencePojisteni.Controllers
{
    [Authorize]
    public class InsuredsController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;

        public InsuredsController(UserManager<IdentityUser> userManager,
			                    SignInManager<IdentityUser> signInManager,
									ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        // GET: Insureds
        public async Task<IActionResult> Index()
		{
			if (User.IsInRole("admin"))
			{
				var insureds = await _context.Insured.ToListAsync();
				return View(insureds);
			}
			else
			{
				var userId = User.Identity.Name;
				var insured = await _context.Insured.Where(i => i.Email == userId).ToListAsync();
				if (insured != null)
				{
					return View(insured);
				}
				else
				{
					return Problem("Pojištěnec nebyl nalezen.");
				}
			}
		}


		// GET: Insureds/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Insured == null)
            {
                return NotFound();
            }

            var insured = await _context.Insured
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insured == null)
            {
                return NotFound();
            }

			if (!User.IsInRole("admin") && User.Identity.Name != insured.Email)
			{
				return RedirectToAction("Index", "Insureds");
			}

			TempData["InsuredId"] = insured.Id;

            return View(insured);
        }


        [Authorize(Roles = "admin")]
        // GET: Insureds/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Insureds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Password,ConfirmPassword,Phone,Street,City,PostalCode")] Insured insured)
        {

            if (ModelState.IsValid)
            {

                var user = new IdentityUser { UserName = insured.Email, Email = insured.Email };
                var result = await _userManager.CreateAsync(user, insured.Password);

                if (result.Succeeded)
                {

                    _context.Add(insured);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Pojištěnec byl úspěšně vytvořen.";
                    TempData["AlertType"] = "success";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(insured);
        }

    

        // GET: Insureds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Insured == null)
            {
                return NotFound();
            }

            var insured = await _context.Insured.FindAsync(id);
            if (insured == null)
            {
                return NotFound();
            }

			TempData["Email"] = insured.Email;

			return View(insured);
        }


        // POST: Insureds/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Password,ConfirmPassword,Phone,Street,City,PostalCode")] Insured insured)
		{
			if (id != insured.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(insured);
					await _context.SaveChangesAsync();


					if (TempData.ContainsKey("Email"))
					{
						string email = TempData["Email"].ToString();

						TempData.Keep();

                        if (email != insured.Email)
                        {
                            IdentityUser user = await _userManager.FindByEmailAsync(email);
                            if (user != null)
                            {
                                user.UserName = insured.Email;
                                user.Email = insured.Email;
                                await _userManager.UpdateAsync(user); 

                                bool isAdmin = User.IsInRole("admin");

                                // Pokud je přihlášený uživatel administrátor, neprovádíme odhlášení
                                if (!isAdmin)
                                {
                                    // Odhlásit uživatele a přesměrovat na přihlašovací stránku
                                    await _signInManager.SignOutAsync();
                                    return RedirectToAction("Login", "Account", new { message = "Byl jste odhlášen z důvodu změny e-mailu. Přihlaste se prosím znovu." });
                                }
                            }
                        }
                    }
                }
				catch (DbUpdateConcurrencyException)
				{
					if (!InsuredExists(insured.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

                TempData["AlertType"] = "success";
                TempData["Message"] = "Pojištěnec byl úspěšně upraven.";

                return RedirectToAction(nameof(Index));
			}

            TempData["AlertType"] = "danger";
            TempData["Message"] = "Pojištěnce se nepodařilo upravit.";

            return View(insured);
		}


		// GET: Insureds/Delete/5
		[Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Insured == null)
            {
                return NotFound();
            }

            var insured = await _context.Insured
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insured == null)
            {
                return NotFound();
            }

            return View(insured);
        }


        // POST: Insureds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Insured == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Insured'  is null.");
            }
            var insured = await _context.Insured.FindAsync(id);
            if (insured != null)
            {
                IdentityUser user = await _userManager.FindByEmailAsync(insured.Email);
                if (user != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(user);
                }
                _context.Insured.Remove(insured);
            }
            
            await _context.SaveChangesAsync();

            TempData["AlertType"] = "success";
            TempData["Message"] = "Pojištěnec byl úspěšně odstraněn.";

            return RedirectToAction(nameof(Index));
        }

        private bool InsuredExists(int id)
        {
          return (_context.Insured?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
