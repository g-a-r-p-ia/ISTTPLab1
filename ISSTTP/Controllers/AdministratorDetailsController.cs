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
    public class AdministratorDetailsController : Controller
    {
        private readonly DbcarShopContext _context;

        public AdministratorDetailsController(DbcarShopContext context)
        {
            _context = context;
        }

        // GET: AdministratorDetails
        public async Task<IActionResult> Index()
        {
            var dbcarShopContext = _context.AdministratorDetails.Include(a => a.Administrator).Include(a => a.Detail);
            return View(await dbcarShopContext.ToListAsync());
        }

        // GET: AdministratorDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administratorDetail = await _context.AdministratorDetails
                .Include(a => a.Administrator)
                .Include(a => a.Detail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administratorDetail == null)
            {
                return NotFound();
            }

            return View(administratorDetail);
        }

        // GET: AdministratorDetails/Create
        public IActionResult Create()
        {
            ViewData["AdministratorId"] = new SelectList(_context.Administarators, "Id", "Id");
            ViewData["DetailId"] = new SelectList(_context.Details, "Id", "Id");
            return View();
        }

        // POST: AdministratorDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AdministratorId,DetailId")] AdministratorDetail administratorDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administratorDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdministratorId"] = new SelectList(_context.Administarators, "Id", "Id", administratorDetail.AdministratorId);
            ViewData["DetailId"] = new SelectList(_context.Details, "Id", "Id", administratorDetail.DetailId);
            return View(administratorDetail);
        }

        // GET: AdministratorDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administratorDetail = await _context.AdministratorDetails.FindAsync(id);
            if (administratorDetail == null)
            {
                return NotFound();
            }
            ViewData["AdministratorId"] = new SelectList(_context.Administarators, "Id", "Id", administratorDetail.AdministratorId);
            ViewData["DetailId"] = new SelectList(_context.Details, "Id", "Name", administratorDetail.DetailId);
            return View(administratorDetail);
        }

        // POST: AdministratorDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AdministratorId,DetailId")] AdministratorDetail administratorDetail)
        {
            if (id != administratorDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administratorDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministratorDetailExists(administratorDetail.Id))
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
            ViewData["AdministratorId"] = new SelectList(_context.Administarators, "Id", "Id", administratorDetail.AdministratorId);
            ViewData["DetailId"] = new SelectList(_context.Details, "Id", "Id", administratorDetail.DetailId);
            return View(administratorDetail);
        }

        // GET: AdministratorDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administratorDetail = await _context.AdministratorDetails
                .Include(a => a.Administrator)
                .Include(a => a.Detail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administratorDetail == null)
            {
                return NotFound();
            }

            return View(administratorDetail);
        }

        // POST: AdministratorDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var administratorDetail = await _context.AdministratorDetails.FindAsync(id);
            if (administratorDetail != null)
            {
                _context.AdministratorDetails.Remove(administratorDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministratorDetailExists(int id)
        {
            return _context.AdministratorDetails.Any(e => e.Id == id);
        }
    }
}
