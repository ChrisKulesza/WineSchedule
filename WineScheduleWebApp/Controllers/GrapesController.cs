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
                .Where(g => g.ApplicationUserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
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
            return View(grape);
        }

        // GET: Grapes/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Grapes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ApplicationUserId")] Grape grape)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                grape.ApplicationUserId = userId;
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
