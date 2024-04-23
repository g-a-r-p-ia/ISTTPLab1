using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISSTTP.Data;

namespace ISSTTP.Controllers
{
    public class AdministaratorsController : Controller
    {
        private readonly DbcarShopContext _context;

        public AdministaratorsController(DbcarShopContext context)
        {
            _context = context;
        }

        // GET: Administarators
        public async Task<IActionResult> Index()
        {
            return View(await _context.Administarators.ToListAsync());
        }

        // GET: Administarators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administarator = await _context.Administarators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administarator == null)
            {
                return NotFound();
            }

            return View(administarator);
        }

        // GET: Administarators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administarators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Administarator administarator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administarator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(administarator);
        }

        // GET: Administarators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administarator = await _context.Administarators.FindAsync(id);
            if (administarator == null)
            {
                return NotFound();
            }
            return View(administarator);
        }

        // POST: Administarators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Administarator administarator)
        {
            if (id != administarator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administarator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministaratorExists(administarator.Id))
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
            return View(administarator);
        }

        // GET: Administarators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administarator = await _context.Administarators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administarator == null)
            {
                return NotFound();
            }

            return View(administarator);
        }

        // POST: Administarators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var administarator = await _context.Administarators.FindAsync(id);
            if (administarator != null)
            {
                _context.Administarators.Remove(administarator);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministaratorExists(int id)
        {
            return _context.Administarators.Any(e => e.Id == id);
        }
    }
}
