using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WineScheduleWebApp.Data;
using WineScheduleWebApp.Models;

namespace WineScheduleWebApp.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Wines")]
    public class WinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Wines
        [HttpGet]
        public IEnumerable<Wine> GetWine()
        {
            return _context.Wine;
        }

        // GET: api/Wines/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWine([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wine = await _context.Wine.SingleOrDefaultAsync(m => m.Id == id);

            if (wine == null)
            {
                return NotFound();
            }

            return Ok(wine);
        }

        // PUT: api/Wines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWine([FromRoute] string id, [FromBody] Wine wine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wine.Id)
            {
                return BadRequest();
            }

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

            return NoContent();
        }

        // PUT: api/Wines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> IncreaseWineStockByOne([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Wine wine = await _context.Wine.FirstOrDefaultAsync(w => w.Id == id);
            if(wine != null)
            {
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
            }
            return NoContent();
        }
        // PUT: api/Wines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> DecreaseWineStockByOne([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Wine wine = await _context.Wine.FirstOrDefaultAsync(w => w.Id == id);
            if (wine != null)
            {
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
            }
            return NoContent();
        }


        // POST: api/Wines
        [HttpPost]
        public async Task<IActionResult> PostWine([FromBody] Wine wine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Wine.Add(wine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWine", new { id = wine.Id }, wine);
        }

        // DELETE: api/Wines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWine([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wine = await _context.Wine.SingleOrDefaultAsync(m => m.Id == id);
            if (wine == null)
            {
                return NotFound();
            }

            _context.Wine.Remove(wine);
            await _context.SaveChangesAsync();

            return Ok(wine);
        }

        private bool WineExists(string id)
        {
            return _context.Wine.Any(e => e.Id == id);
        }
    }
}