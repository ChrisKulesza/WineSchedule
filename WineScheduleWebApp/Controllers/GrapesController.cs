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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WineScheduleWebApp.Controllers
{
    [Authorize]
    public class GrapesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GrapesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Grapes
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var grapes = await _context.Grape
                .Include(g => g.Category)
                .Where(g => g.ApplicationUserId == userId)
                .ToListAsync();
            return View(grapes);
        }

        // GET: Grapes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grape = await _context.Grape
                .Include(g => g.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (grape == null)
            {
                return NotFound();
            }
            ViewBag.ApplicationUserName = await _context.ApplicationUser
                .SingleOrDefaultAsync(a => a.Id == grape.ApplicationUserId);
            ViewBag.WineGrapes = await _context.WineGrape
                .Include(wg => wg.Wine)
                .Where(wg => wg.GrapeId == grape.Id)
                .ToListAsync();
            ViewBag.Categories = await _context.Category
                .Where(c => c.ApplicationUserId == grape.ApplicationUserId)
                .ToListAsync();
            return View(grape);
        }

        // GET: Grapes/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Categories = new SelectList(_context.Category
                .Where(w => w.ApplicationUserId == userId), "Id", "Name", "0");
            var grape = new Grape() { ApplicationUserId = userId };
            return View(grape);
        }

        // POST: Grapes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ApplicationUserId,CategoryId")] Grape grape)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grape);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(grape);
        }

        // GET: Grapes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grape = await _context.Grape.SingleOrDefaultAsync(m => m.Id == id);
            if (grape == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.Category
                .Where(c => c.ApplicationUserId == grape.ApplicationUserId), "Id", "Name", grape.CategoryId);
            return View(grape);
        }

        // POST: Grapes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,ApplicationUserId,CreationDate,LastModifiedDate")] Grape grape)
        {
            if (id != grape.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grape);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrapeExists(grape.Id))
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
            ViewBag.Categories = new SelectList(_context.Category
                .Where(c => c.ApplicationUserId == grape.ApplicationUserId), "Id", "Name", grape.CategoryId);
            return View(grape);
        }

        // GET: Grapes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grape = await _context.Grape
                .SingleOrDefaultAsync(m => m.Id == id);
            if (grape == null)
            {
                return NotFound();
            }

            return View(grape);
        }

        // POST: Grapes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var grape = await _context.Grape.SingleOrDefaultAsync(m => m.Id == id);
            _context.Grape.Remove(grape);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool GrapeExists(string id)
        {
            return _context.Grape.Any(e => e.Id == id);
        }
    }
}
