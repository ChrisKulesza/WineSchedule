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
using WineScheduleWebApp.Models.WineViewModels;
using WineScheduleWebApp.HelperModels;

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
                .Include(w => w.Dryness)
                .Include(w => w.Category)
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
                .Include(w => w.Appellation)
                .Include(w => w.Region)
                .Include(w => w.Dryness)
                .Include(w => w.Category)
                .Include(w => w.WineGrapes)
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
            // Lists to choose
            ViewBag.Appellations = new SelectList(_context.Appellation, "Id", "Name");
            ViewBag.Regions = new SelectList(_context.Region, "Id", "Name");
            ViewBag.Drynesses = new SelectList(_context.Dryness, "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Category, "Id", "Name");

            // Fill in the view model
            var grapes = _context.Grape.ToList();
            var createWineViewModel = new CreateWineViewModel();
            for (int i = 0; i < grapes.Count(); i++)
            {
                // Fill out the check boxes to select the grapes
                createWineViewModel.IdCheckBoxes.Add(new IdCheckBox() { Id = grapes[i].Id, Name = grapes[i].Name, IsSelected = false });
            }
            return View(createWineViewModel);
        }

        // POST: Wines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWineViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                
                Wine wine = viewModel.Wine;

                wine.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(wine);

                List<IdCheckBox> idCheckBoxes = viewModel.IdCheckBoxes;
                foreach (var idCheckBox in idCheckBoxes)
                {
                    if(idCheckBox.IsSelected)
                    {
                        _context.Add(new WineGrape() { WineId = wine.Id, GrapeId = idCheckBox.Id });
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Fill in the view model
            var grapes = _context.Grape.ToList();
            var createWineViewModel = new CreateWineViewModel();
            for (int i = 0; i < grapes.Count(); i++)
            {
                // Fill out the check boxes to select the grapes
                createWineViewModel.IdCheckBoxes.Add(new IdCheckBox() { Id = grapes[i].Id, Name = grapes[i].Name, IsSelected = false });
            }

            ViewBag.Appellations = new SelectList(_context.Appellation, "Id", "Name", viewModel.Wine.AppellationId);
            ViewBag.Regions = new SelectList(_context.Region, "Id", "Name", viewModel.Wine.RegionId);
            ViewBag.Drynesses = new SelectList(_context.Dryness, "Id", "Name", viewModel.Wine.DrynessId);
            ViewBag.Categories = new SelectList(_context.Category, "Id", "Name", viewModel.Wine.CategoryId);
            return View(viewModel);
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

            var wine = await _context.Wine
                .Include(w => w.Appellation)
                .Include(w => w.Region)
                .Include(w => w.Dryness)
                .Include(w => w.Category)
                .Include(w => w.WineGrapes)
                .SingleOrDefaultAsync(m => m.Id == id); 
            if (wine == null)
            {
                return NotFound();
            }
            var editWineViewModel = new EditWineViewModel();
            editWineViewModel.Wine = wine;
            var grapes = _context.Grape.ToList();
            var wineGrapes = _context.WineGrape.Where(wg => wg.GrapeId == wine.Id).ToList();
            editWineViewModel.IdCheckBoxes = GetEditCheckBoxes(grapes, wineGrapes);
            

            // Data to list
            ViewBag.Appellations = new SelectList(_context.Appellation, "Id", "Name", wine.AppellationId);
            ViewBag.Regions = new SelectList(_context.Region, "Id", "Name", wine.RegionId);
            ViewBag.Drynesses = new SelectList(_context.Dryness, "Id", "Name", wine.DrynessId);
            ViewBag.Categories = new SelectList(_context.Category, "Id", "Name", wine.CategoryId);
            return View(editWineViewModel);
        }

        // POST: Wines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditWineViewModel viewModel)
        {
            if (id != viewModel.Wine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Wine wine = viewModel.Wine;
                // Add each Wine to Grape relationship to the database
                List<IdCheckBox> idCheckBoxes = viewModel.IdCheckBoxes;
                foreach (var idCheckBox in idCheckBoxes)
                {
                    if(idCheckBox.IsSelected)
                    {
                        WineGrape wineGrape = new WineGrape() { WineId = wine.Id, GrapeId = idCheckBox.Id };
                        _context.Add(wineGrape);
                    }
                }
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
            ViewBag.Appellations = new SelectList(_context.Appellation, "Id", "Name", viewModel.Wine.AppellationId);
            ViewBag.Regions = new SelectList(_context.Region, "Id", "Name", viewModel.Wine.RegionId);
            ViewBag.Drynesses = new SelectList(_context.Dryness, "Id", "Name", viewModel.Wine.DrynessId);
            ViewBag.Categories = new SelectList(_context.Category, "Id", "Name", viewModel.Wine.CategoryId);
            return View(viewModel);
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
        private List<IdCheckBox> GetEditCheckBoxes(List<Grape> grapes, List<WineGrape> wineGrapes)
        {
            var idCheckBoxes = new List<IdCheckBox>();
            bool isSelected = false;
            for (int i = 0; i < grapes.Count(); i++)
            {
                
                for (int j = 0; j < wineGrapes.Count(); j++)
                {
                    if (grapes[i].Id == wineGrapes[i].GrapeId)
                    {
                        isSelected = true;
                        break;
                    }
                }
                idCheckBoxes.Add(new IdCheckBox() { Id = grapes[i].Id, Name = grapes[i].Name, IsSelected = isSelected });
            }
            return idCheckBoxes;
        }
    }
}
