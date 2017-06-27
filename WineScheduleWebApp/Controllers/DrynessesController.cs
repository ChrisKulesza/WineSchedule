using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WineScheduleWebApp.Data;
using WineScheduleWebApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WineScheduleWebApp.Controllers
{
    public class DrynessesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DrynessesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Drynesses
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var drynesses = await _context.Dryness
                .Where(d => d.ApplicationUserId == userId)
                .ToListAsync();
            return View(drynesses);
        }

        // GET: Drynesses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dryness = await _context.Dryness
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dryness == null)
            {
                return NotFound();
            }
            ViewBag.ApplicationUserName = await _context.ApplicationUser
                .SingleOrDefaultAsync(a => a.Id == dryness.ApplicationUserId);
            ViewBag.Wines = await _context.Wine
                .Where(w => w.DrynessId == id)
                .ToListAsync();
            return View(dryness);
        }

        // GET: Drynesses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drynesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Dryness dryness)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                dryness.ApplicationUserId = userId;
                _context.Add(dryness);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dryness);
        }

        // GET: Drynesses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dryness = await _context.Dryness.SingleOrDefaultAsync(m => m.Id == id);
            if (dryness == null)
            {
                return NotFound();
            }
            return View(dryness);
        }

        // POST: Drynesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,ApplicationUserId")] Dryness dryness)
        {
            if (id != dryness.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dryness);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrynessExists(dryness.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(dryness);
        }

        // GET: Drynesses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dryness = await _context.Dryness
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dryness == null)
            {
                return NotFound();
            }

            return View(dryness);
        }

        // POST: Drynesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dryness = await _context.Dryness.SingleOrDefaultAsync(m => m.Id == id);
            _context.Dryness.Remove(dryness);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DrynessExists(string id)
        {
            return _context.Dryness.Any(e => e.Id == id);
        }
    }
}
