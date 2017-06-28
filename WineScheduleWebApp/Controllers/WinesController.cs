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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wines = await _context.Wine
                .Include(w => w.Appellation)
                .Include(w => w.Region)
                .Include(w => w.Dryness)
                .Include(w => w.Category)
                .Include(w => w.WineGrapes)
                .Where(w => w.ApplicationUserId == userId)
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
                .Include(w => w.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wine == null)
            {
                return NotFound();
            }
            ViewBag.WineGrapes = await _context.WineGrape
                .Include(wg => wg.Grape)
                .Where(wg => wg.WineId == wine.Id) //  Not neccessary -> && wg.ApplicationUserId == wine.ApplicationUserId
                .ToListAsync();

            return View(wine);
        }

        // GET: Wines/Create
        public async Task<IActionResult> Create()
        {
            // Lists to choose
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Appellations = new SelectList(_context.Appellation
                .Where(w => w.ApplicationUserId == userId), "Id", "Name");
            ViewBag.Regions = new SelectList(_context.Region
                .Where(w => w.ApplicationUserId == userId), "Id", "Name");
            ViewBag.Drynesses = new SelectList(_context.Dryness
                .Where(w => w.ApplicationUserId == userId), "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Category
                .Where(w => w.ApplicationUserId == userId), "Id", "Name");

            // Fill in the view model
            var grapes = await _context.Grape
                .Where(g => g.ApplicationUserId == userId)
                .ToListAsync();
            var createWineViewModel = new CreateWineViewModel()
            {
                Wine = new Wine()
                {
                    ApplicationUserId = userId
                }
            };
            for (int i = 0; i < grapes.Count(); i++)
            {
                // Fill out the check boxes to select the grapes
                createWineViewModel.IdCheckBoxes.Add(new IdCheckBox()
                {
                    Id = grapes[i].Id,
                    Name = grapes[i].Name,
                    IsSelected = false
                });
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
                _context.Add(wine);

                List<IdCheckBox> idCheckBoxes = viewModel.IdCheckBoxes;
                foreach (var idCheckBox in idCheckBoxes)
                {
                    if(idCheckBox.IsSelected)
                    {
                        _context.Add(new WineGrape()
                        {
                            ApplicationUserId = viewModel.Wine.ApplicationUserId,
                            WineId = wine.Id,
                            GrapeId = idCheckBox.Id
                        });
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
            
            ViewBag.Appellations = new SelectList(_context.Appellation
                .Where(w => w.ApplicationUserId == viewModel.Wine.ApplicationUserId), "Id", "Name", viewModel.Wine.AppellationId);
            ViewBag.Regions = new SelectList(_context.Region
                .Where(w => w.ApplicationUserId == viewModel.Wine.ApplicationUserId), "Id", "Name", viewModel.Wine.RegionId);
            ViewBag.Drynesses = new SelectList(_context.Dryness
                .Where(w => w.ApplicationUserId == viewModel.Wine.ApplicationUserId), "Id", "Name", viewModel.Wine.DrynessId);
            ViewBag.Categories = new SelectList(_context.Category
                .Where(w => w.ApplicationUserId == viewModel.Wine.ApplicationUserId), "Id", "Name", viewModel.Wine.CategoryId);
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
            var grapes = _context.Grape.ToList();
            var wineGrapes = _context.WineGrape.Where(wg => wg.WineId == wine.Id).ToList();
            var editWineViewModel = new EditWineViewModel() {
                Wine = wine,
                IdCheckBoxes = GetEditCheckBoxes(grapes, wineGrapes)
            };
              
            // Data to list
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Appellations = new SelectList(_context.Appellation
                .Where(w => w.ApplicationUserId == userId), "Id", "Name", wine.AppellationId);
            ViewBag.Regions = new SelectList(_context.Region
                .Where(w => w.ApplicationUserId == userId), "Id", "Name", wine.RegionId);
            ViewBag.Drynesses = new SelectList(_context.Dryness
                .Where(w => w.ApplicationUserId == userId), "Id", "Name", wine.DrynessId);
            ViewBag.Categories = new SelectList(_context.Category
                .Where(w => w.ApplicationUserId == userId), "Id", "Name", wine.CategoryId);
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
                // TODO: Testing list comparison
                List<string> databaseGrapeIds = _context.WineGrape
                    .Where(wg => wg.WineId == wine.Id)
                    .Select(wg => wg.GrapeId)
                    .ToList();
                List<string> selectedGrapeIds = viewModel.IdCheckBoxes
                    .Where(cb => cb.IsSelected == true)
                    .Select(cb => cb.Id)
                    .ToList();

                // Except the lists to determine the WineGrapes which must be removed
                var exceptRemoveGrapeIds = Enumerable.Except(databaseGrapeIds, selectedGrapeIds); //{1,2,3} intersect {3,4,5} = {1,2}
                // Delete all WineGrapes which are in the DB but not ticked in the edit view
                foreach (var exceptGrapeId in exceptRemoveGrapeIds)
                {
                    var wineGrape = await _context.WineGrape
                        .SingleOrDefaultAsync(wg => wg.WineId == wine.Id && wg.GrapeId == exceptGrapeId);
                    if(wineGrape != null) _context.WineGrape.Remove(wineGrape);
                }
                // Intersect the lists to determine the WineGrapes which must be added
                var exceptAddGrapeIds = Enumerable.Except(selectedGrapeIds, databaseGrapeIds); //{3,4,5} intersect {1,2,3} = {3,4} 
                foreach (var exceptGrapeId in exceptAddGrapeIds)
                {
                    _context.WineGrape.Add(new WineGrape()
                    {
                        WineId = wine.Id,
                        GrapeId = exceptGrapeId
                    });
                }
                //var unionGrapeIds = Enumerable.Union(selectedGrapeIds, intersectGrapeIds);
                ////foreach
                //var list1 = new List<string>() { "123", "456", "789" };
                //var list2 = new List<string>() { "123", "463", "938", "298" };
                //var test1 = Enumerable.SequenceEqual(list1, list2);
                //var test2 = Enumerable.Union(list1, list2);
                //var test3 = Enumerable.Concat(list1, list2);
                //var test4 = Enumerable.Intersect(list1, list2);
                //var test3 = Enumerable.Zip(selectedGrapeIds, grapeIds, ((list1, list2, list3) =>

                // Intersect -> delete all others from list1
                // list2 union intersect result

                //Wine wine = viewModel.Wine;
                //// Get the selected grapes (idCheckBoxes) from the ViewModel
                //List<IdCheckBox> idCheckBoxes = viewModel.IdCheckBoxes;
                //// Get the previous selected grapes from the database
                //List<WineGrape> wineGrapes = _context
                //    .WineGrape.Where(wg => wg.WineId == wine.Id)
                //    .ToList();
                //Add each Wine to Grape relationship to the db
                //foreach (var idCheckBox in idCheckBoxes)
                //{
                //    if(idCheckBox.IsSelected)
                //    {
                //        // Check if the selected grape is already in the database
                //        bool isFound = false;
                //        foreach (var wineGrape in wineGrapes)
                //        {
                //            if(wineGrape.GrapeId == idCheckBox.Id)
                //            {
                //                isFound = true;
                //                break;
                //            }
                //        }
                //        // Add to the database if the grape wine relationship doesn't exist in the db
                //        if(!isFound)
                //        {
                //            var wineGrape = new WineGrape() { WineId = wine.Id, GrapeId = idCheckBox.Id };
                //            _context.Add(wineGrape);
                //        }
                //    }
                //}
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Appellations = new SelectList(_context.Appellation
                .Where(a => a.ApplicationUserId == userId), "Id", "Name", viewModel.Wine.AppellationId);
            ViewBag.Regions = new SelectList(_context.Region
                .Where(r => r.ApplicationUserId == userId), "Id", "Name", viewModel.Wine.RegionId);
            ViewBag.Drynesses = new SelectList(_context.Dryness
                .Where(d => d.ApplicationUserId == userId), "Id", "Name", viewModel.Wine.DrynessId);
            ViewBag.Categories = new SelectList(_context.Category
                .Where(c => c.ApplicationUserId == userId), "Id", "Name", viewModel.Wine.CategoryId);
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
                    if (grapes[i].Id == wineGrapes[j].GrapeId)
                    {
                        isSelected = true;
                        break;
                    }
                }
                idCheckBoxes.Add(new IdCheckBox() { Id = grapes[i].Id, Name = grapes[i].Name, IsSelected = isSelected });
                isSelected = false;
            }
            return idCheckBoxes;
        }
    }
}
