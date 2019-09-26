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

namespace WineScheduleWebApp.Controllers.Api
{
    public class AppellationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppellationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Appellations
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appellations = await _context.Appellation
                .Include(a => a.Region)
                .Where(a => a.ApplicationUserId == userId)
                .ToListAsync();
            return View(appellations);
        }

        // GET: Appellations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appellation = await _context.Appellation
                .Include(a => a.Region)
                .Include(a => a.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appellation == null)
            {
                return NotFound();
            }
            ViewBag.ApplicationUserName = await _context.ApplicationUser
                .SingleOrDefaultAsync(a => a.Id == appellation.ApplicationUserId);
            ViewBag.Wines = await _context.Wine
                .Where(w => w.AppellationId == id)
                .ToListAsync();
            return View(appellation);
        }

        // GET: Appellations/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appellation = new Appellation() { ApplicationUserId = userId };
            ViewBag.Regions = new SelectList(_context.Region
                .Where(r => r.ApplicationUserId == userId), "Id", "Name");
            return View(appellation);
        }

        // POST: Appellations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,RegionId,ApplicationUserId")] Appellation appellation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appellation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Regions = new SelectList(_context.Region
                .Where(r => r.ApplicationUserId == appellation.ApplicationUserId), "Id", "Name", appellation.RegionId);
            return View(appellation);
        }

        // GET: Appellations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appellation = await _context.Appellation
                .Include(a => a.Region)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appellation == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Regions = new SelectList(_context.Region
                .Where(r => r.ApplicationUserId == userId), "Id", "Name", appellation.RegionId);
            return View(appellation);
        }

        // POST: Appellations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,ApplicationUserId,RegionId")] Appellation appellation)
        {
            if (id != appellation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appellation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppellationExists(appellation.Id))
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Regions = new SelectList(_context.Region
                .Where(r => r.ApplicationUserId == userId), "Id", "Name", appellation.RegionId);
            return View(appellation);
        }

        // GET: Appellations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appellation = await _context.Appellation
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appellation == null)
            {
                return NotFound();
            }

            return View(appellation);
        }

        // POST: Appellations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var appellation = await _context.Appellation.SingleOrDefaultAsync(m => m.Id == id);
            _context.Appellation.Remove(appellation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AppellationExists(string id)
        {
            return _context.Appellation.Any(e => e.Id == id);
        }
    }
}
