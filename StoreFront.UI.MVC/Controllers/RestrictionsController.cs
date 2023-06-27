using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;

namespace StoreFront.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RestrictionsController : Controller
    {
        private readonly StoreFrontContext _context;

        public RestrictionsController(StoreFrontContext context)
        {
            _context = context;
        }

        // GET: Restrictions
        public async Task<IActionResult> Index()
        {
              return _context.Restrictions != null ? 
                          View(await _context.Restrictions.ToListAsync()) :
                          Problem("Entity set 'StoreFrontContext.Restrictions'  is null.");
        }

        // GET: Restrictions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Restrictions == null)
            {
                return NotFound();
            }

            var restriction = await _context.Restrictions
                .FirstOrDefaultAsync(m => m.RestrictionId == id);
            if (restriction == null)
            {
                return NotFound();
            }

            return View(restriction);
        }

        // GET: Restrictions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restrictions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestrictionId,RestrictionType,PermitNeeded")] Restriction restriction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restriction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restriction);
        }

        // GET: Restrictions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Restrictions == null)
            {
                return NotFound();
            }

            var restriction = await _context.Restrictions.FindAsync(id);
            if (restriction == null)
            {
                return NotFound();
            }
            return View(restriction);
        }

        // POST: Restrictions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RestrictionId,RestrictionType,PermitNeeded")] Restriction restriction)
        {
            if (id != restriction.RestrictionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restriction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestrictionExists(restriction.RestrictionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(restriction);
        }

        // GET: Restrictions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Restrictions == null)
            {
                return NotFound();
            }

            var restriction = await _context.Restrictions
                .FirstOrDefaultAsync(m => m.RestrictionId == id);
            if (restriction == null)
            {
                return NotFound();
            }

            return View(restriction);
        }

        // POST: Restrictions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Restrictions == null)
            {
                return Problem("Entity set 'StoreFrontContext.Restrictions'  is null.");
            }
            var restriction = await _context.Restrictions.FindAsync(id);
            if (restriction != null)
            {
                _context.Restrictions.Remove(restriction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestrictionExists(int id)
        {
          return (_context.Restrictions?.Any(e => e.RestrictionId == id)).GetValueOrDefault();
        }
    }
}
