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
    public class WinesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WinesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Wines
        public async Task<IActionResult> Index()
        {
            var wines = await _context.Wine
                .Include(w => w.Appellation)
                .Include(w => w.Region)
                .Include(w => w.WineGrapes)
                .ToListAsync();
            return View(wines);
        }

        // GET: Wines/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wine = await _context.Wine
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wine == null)
            {
                return NotFound();
            }
            ViewBag.RegionName = await _context.Region
                .SingleOrDefaultAsync(r => r.Id == wine.RegionId);

            return View(wine);
        }

        // GET: Wines/Create
        public IActionResult Create()
        {
            ViewBag.Regions = new SelectList(_context.Region, "Id", "Name");
            return View();
        }

        // POST: Wines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Year,Price,Rating,Stock,ImagePath,Description,ApplicationUserId,CreationDate,LastModifiedDate")] Wine wine)
        {
            if (ModelState.IsValid)
            {
                wine.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(wine);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Regions = new SelectList(_context.Region, "Id", "Name", wine.RegionId);
            return View(wine);
        }

        // GET: Wines/InceaseStock/5
        public async Task<IActionResult> IncreaseStock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wine = await _context.Wine.SingleOrDefaultAsync(w => w.Id == id);
            if (wine == null)
            {
                return NotFound();
            }
            if(wine.Stock > 9999)
            {
                return RedirectToAction("Index"); ;
            }
            wine.Stock += 1;
            _context.Entry(wine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WineExists(id))
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

        // GET: Wines/DecreaseStock/5
        public async Task<IActionResult> DecreaseStock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wine = await _context.Wine.SingleOrDefaultAsync(w => w.Id == id);
            if (wine == null)
            {
                return NotFound();
            }
            if (wine.Stock < 1)
            {
                return RedirectToAction("Index"); ;
            }
            wine.Stock -= 1;
            _context.Entry(wine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WineExists(id))
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
        // GET: Wines/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wine = await _context.Wine.SingleOrDefaultAsync(m => m.Id == id);
            if (wine == null)
            {
                return NotFound();
            }
            ViewBag.Regions = new SelectList(_context.Region, "Id", "Name", wine.RegionId);
            return View(wine);
        }

        // POST: Wines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Year,Price,Rating,Stock,ImagePath,Description,ApplicationUserId,CreationDate,LastModifiedDate")] Wine wine)
        {
            if (id != wine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WineExists(wine.Id))
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
            ViewBag.Regions = new SelectList(_context.Region, "Id", "Name", wine.RegionId);
            return View(wine);
        }

        // GET: Wines/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wine = await _context.Wine
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wine == null)
            {
                return NotFound();
            }

            return View(wine);
        }

        // POST: Wines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var wine = await _context.Wine.SingleOrDefaultAsync(m => m.Id == id);
            _context.Wine.Remove(wine);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool WineExists(string id)
        {
            return _context.Wine.Any(e => e.Id == id);
        }
    }
}
