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
using Microsoft.AspNetCore.Authorization;

namespace WineScheduleWebApp.Controllers
{
    [Authorize]
    public class RecordsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecordsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Records
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Record.Include(r => r.Wine);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Records/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Record
                .Include(r => r.Wine)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            ViewBag.ApplicationUserName = await _context.ApplicationUser
                .SingleOrDefaultAsync(a => a.Id == record.ApplicationUserId);
            ViewBag.WineName = await _context.Wine
                .SingleOrDefaultAsync(w => w.Id == record.WineId);
            return View(record);
        }

        // GET: Records/Create
        public IActionResult Create()
        {
            ViewBag.Wines = new SelectList(_context.Wine, "Id", "Name");
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WineId,Price,Quantity,ApplicationUserId,CreationDate,LastModifiedDate")] Record record)
        {
            if (ModelState.IsValid)
            {
                record.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                Wine wine = await _context.Wine.SingleOrDefaultAsync(w => w.Id == record.WineId);
                // If wine is not found, then the stock will be incorrect.
                // Determine save action and reload page
                if (wine != null)
                {
                    // Calculate the total price of the record: quantity * wine prive
                    record.Price = record.Quantity * wine.Price;
                    // Calculate the wine stock: acutal wine stock * new record stock
                    wine.Stock += record.Quantity;
                    _context.Add(record);
                    try
                    {
                        _context.Update(wine);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RecordExists(wine.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.Wines = new SelectList(_context.Wine, "Id", "Name", record.WineId);
            return View(record);
        }

        // GET: Records/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Record.SingleOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            ViewBag.Wines = new SelectList(_context.Wine, "Id", "Name", record.WineId);
            return View(record);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,WineId,Price,Quantity,ApplicationUserId,CreationDate,LastModifiedDate")] Record record)
        {
            if (id != record.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordExists(record.Id))
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
            ViewBag.Wines = new SelectList(_context.Wine, "Id", "Name", record.WineId);
            return View(record);
        }

        // GET: Records/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Record
                .Include(r => r.Wine)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var record = await _context.Record.SingleOrDefaultAsync(m => m.Id == id);
            _context.Record.Remove(record);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RecordExists(string id)
        {
            return _context.Record.Any(e => e.Id == id);
        }
    }
}
