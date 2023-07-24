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

namespace EvidencePojisteni.Controllers
{
	[Authorize]
	public class InsurancesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public InsurancesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Insurances
		public async Task<IActionResult> Index()
		{
			if (User.IsInRole("admin"))
			{
				var insurances = await _context.Insurance.Include(i => i.Insured).ToListAsync();
				return View(insurances);
			}
			else
			{
				var userId = User.Identity.Name;
				var insurance = await _context.Insurance.Include(i => i.Insured).Where(i => i.Insured.Email == userId).ToListAsync();

				return View(insurance);

			}
		}


		// GET: Insurances/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Insurance == null)
			{
				return NotFound();
			}

			var insurance = await _context.Insurance
				.Include(i => i.Insured)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (insurance == null)
			{
				return NotFound();
			}

			if (!User.IsInRole("admin") && User.Identity.Name != insurance.Insured.Email)
			{
				return RedirectToAction("Index", "Insurances");
			}

			ViewBag.Insured = insurance.Insured;
			TempData["InsuranceId"] = insurance.Id;

			return View(insurance);
		}


		// GET: Insurances/Create
		[Authorize(Roles = "admin")]
		public IActionResult Create()
		{

			if (TempData.ContainsKey("InsuredId"))
			{
				int insuredId = Convert.ToInt32(TempData["InsuredId"].ToString());

				TempData.Keep();

				var insured = _context.Insured.Find(insuredId);
				ViewBag.Insured = insured;
			}

			return View();
		}


		// POST: Insurances/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Create([Bind("Id,Type,Amount,Subject,ValidFrom,ValidUntil,InsuredId")] Insurance insurance)
		{
			if (ModelState.IsValid)
			{
				_context.Add(insurance);
				await _context.SaveChangesAsync();

                TempData["AlertType"] = "success";
                TempData["Message"] = "Pojištění bylo úspěšně vytvořeno.";

                return RedirectToAction(nameof(Index));
			}

			if (TempData.ContainsKey("InsuredId"))
			{
				int insuredId = Convert.ToInt32(TempData["InsuredId"].ToString());

				TempData.Keep();

				var insured = await _context.Insured.FindAsync(insuredId);
				ViewBag.Insured = insured;
			}

            TempData["AlertType"] = "danger";
            TempData["Message"] = "Pojištění se nepodařilo vytvořit.";

            return View(insurance);
		}


		// GET: Insurances/Edit/5
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Insurance == null)
			{
				return NotFound();
			}

			var insurance = await _context.Insurance.FindAsync(id);
			if (insurance == null)
			{
				return NotFound();
			}

			TempData["InsuredId"] = insurance.Insured.Id;
			ViewBag.Insured = insurance.Insured;

			return View(insurance);
		}


		// POST: Insurances/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Amount,Subject,ValidFrom,ValidUntil,InsuredId")] Insurance insurance)
		{
			if (id != insurance.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(insurance);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!InsuranceExists(insurance.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

                TempData["AlertType"] = "success";
                TempData["Message"] = "Pojištění bylo úspěšně upraveno.";

                return RedirectToAction(nameof(Index));
			}

			if (TempData.ContainsKey("InsuredId"))
			{
				int insuredId = Convert.ToInt32(TempData["InsuredId"].ToString());

				TempData.Keep();

				var insured = await _context.Insured.FindAsync(insuredId);
				ViewBag.Insured = insured;
			}

            TempData["AlertType"] = "danger";
            TempData["Message"] = "Pojištění se nepodařilo upravit.";

            return View(insurance);
		}


		// GET: Insurances/Delete/5
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Insurance == null)
			{
				return NotFound();
			}

			var insurance = await _context.Insurance
				.Include(i => i.Insured)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (insurance == null)
			{
				return NotFound();
			}

			var insured = await _context.Insured.FindAsync(insurance.InsuredId);
			if (insured == null)
			{
				return NotFound();
			}
			ViewBag.Insured = insured;

			return View(insurance);
		}


		// POST: Insurances/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Insurance == null)
			{
				return Problem("Entity set 'ApplicationDbContext.Insurance'  is null.");
			}
			var insurance = await _context.Insurance.FindAsync(id);
			if (insurance != null)
			{
				_context.Insurance.Remove(insurance);
			}

			await _context.SaveChangesAsync();

            TempData["AlertType"] = "success";
            TempData["Message"] = "Pojištění bylo úspěšně odstraněno.";

            return RedirectToAction(nameof(Index));
		}

		private bool InsuranceExists(int id)
		{
			return (_context.Insurance?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
