using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WineScheduleWebApp.Data;
using WineScheduleWebApp.Models;

namespace WineScheduleWebApp.Controllers.Api
{
    public class AppellationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppellationsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Appellations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Appellation.ToListAsync());
        }

        // GET: Appellations/Details/5
        public async Task<IActionResult> Details(string id)
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

        // GET: Appellations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appellations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ApplicationUserId,CreationDate,LastModifiedDate")] Appellation appellation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appellation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(appellation);
        }

        // GET: Appellations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appellation = await _context.Appellation.SingleOrDefaultAsync(m => m.Id == id);
            if (appellation == null)
            {
                return NotFound();
            }
            return View(appellation);
        }

        // POST: Appellations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,ApplicationUserId,CreationDate,LastModifiedDate")] Appellation appellation)
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
