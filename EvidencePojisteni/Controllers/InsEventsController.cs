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
	public class InsEventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InsEvents
        public async Task<IActionResult> Index()
        {
			if (User.IsInRole("admin"))
			{
				var insEvents = await _context.Event.Include(i => i.Insurance).ToListAsync();
				return View(insEvents);
			}
			else
			{
				var userId = User.Identity.Name;
				var insEvent = await _context.Event.Include(i => i.Insurance).Where(i => i.Insurance.Insured.Email == userId).ToListAsync();
			    return View(insEvent);

			}
		}


        // GET: InsEvents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Event == null)
            {
                return NotFound();
            }

            var insEvent = await _context.Event
                .Include(i => i.Insurance)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insEvent == null)
            {
                return NotFound();
            }

			if (!User.IsInRole("admin") && User.Identity.Name != insEvent.Insurance.Insured.Email)
			{
				return RedirectToAction("Index", "InsEvents");
			}

			ViewBag.Insurance = insEvent.Insurance;
			ViewBag.Insured = insEvent.Insurance.Insured;

			return View(insEvent);
        }


        // GET: InsEvents/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
			if (TempData.ContainsKey("InsuranceId"))
			{
				int insuranceId = Convert.ToInt32(TempData["InsuranceId"].ToString());

				TempData.Keep();

				var insurance = _context.Insurance.Find(insuranceId);
				int insuredId = insurance.InsuredId;
				var insured = _context.Insured.Find(insuredId);

				ViewBag.Insurance = insurance;
				ViewBag.Insured = insured;
			}
			return View();
        }


        // POST: InsEvents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Create([Bind("Id,Name,Date,Description,Amount,Status,InsuranceId")] InsEvent insEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insEvent);
                await _context.SaveChangesAsync();

                TempData["AlertType"] = "success";
                TempData["Message"] = "Událost byla úspěšně vytvořena.";

                return RedirectToAction(nameof(Index));
            }

			if (TempData.ContainsKey("InsuranceId"))
			{
				int insuranceId = Convert.ToInt32(TempData["InsuranceId"].ToString());

				TempData.Keep();

				var insurance = await _context.Insurance.FindAsync(insuranceId);
				int insuredId = insurance.InsuredId;
				var insured = await _context.Insured.FindAsync(insuredId);

				ViewBag.Insurance = insurance;
				ViewBag.Insured = insured;
			}

            TempData["AlertType"] = "danger";
            TempData["Message"] = "Událost se nepodařilo vytvořit.";

            return View(insEvent);
        }


		// GET: InsEvents/Edit/5
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Event == null)
            {
                return NotFound();
            }

            var insEvent = await _context.Event.FindAsync(id);
            if (insEvent == null)
            {
                return NotFound();
            }

			TempData["InsuranceId"] = insEvent.Insurance.Id;
			TempData["InsuredId"] = insEvent.Insurance.Insured.Id;

			ViewBag.Insurance = insEvent.Insurance;
			ViewBag.Insured = insEvent.Insurance.Insured;

			return View(insEvent);
        }


		// POST: InsEvents/Edit/5
		[HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date,Description,Amount,Status,InsuranceId")] InsEvent insEvent)
        {
            if (id != insEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsEventExists(insEvent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["AlertType"] = "success";
                TempData["Message"] = "Událost byla úspěšně upravena.";

                return RedirectToAction(nameof(Index));
            }

			if (TempData.ContainsKey("InsuranceId"))
			{
				int insuranceId = Convert.ToInt32(TempData["InsuranceId"].ToString());

				TempData.Keep();

				var insurance = await _context.Insurance.FindAsync(insuranceId);
				int insuredId = insurance.InsuredId;
				var insured = await _context.Insured.FindAsync(insuredId);

				ViewBag.Insurance = insurance;
				ViewBag.Insured = insured;
			}

            TempData["AlertType"] = "danger";
            TempData["Message"] = "Událost se nepodařilo upravit.";

            return View(insEvent);
        }


		// GET: InsEvents/Delete/5
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Event == null)
            {
                return NotFound();
            }

            var insEvent = await _context.Event
                .Include(i => i.Insurance)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insEvent == null)
            {
                return NotFound();
            }

			ViewBag.Insurance = insEvent.Insurance;
			ViewBag.Insured = insEvent.Insurance.Insured;

			return View(insEvent);
        }


        // POST: InsEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Event == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Event'  is null.");
            }
            var insEvent = await _context.Event.FindAsync(id);
            if (insEvent != null)
            {
                _context.Event.Remove(insEvent);
            }
            
            await _context.SaveChangesAsync();

            TempData["AlertType"] = "success";
            TempData["Message"] = "Událost byla úspěšně odstraněna.";

            return RedirectToAction(nameof(Index));
        }

        private bool InsEventExists(int id)
        {
          return _context.Event.Any(e => e.Id == id);
        }
    }
}
