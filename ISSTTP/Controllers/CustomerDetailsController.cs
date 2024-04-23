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
    public class CustomerDetailsController : Controller
    {
        private readonly DbcarShopContext _context;

        public CustomerDetailsController(DbcarShopContext context)
        {
            _context = context;
        }

        // GET: CustomerDetails
        public async Task<IActionResult> Index()
        {
            var dbcarShopContext = _context.CustomerDetails.Include(c => c.Customer).Include(c => c.Detail).Include(c => c.Status);
            return View(await dbcarShopContext.ToListAsync());
        }

        // GET: CustomerDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerDetail = await _context.CustomerDetails
                .Include(c => c.Customer)
                .Include(c => c.Detail)
                .Include(c => c.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerDetail == null)
            {
                return NotFound();
            }

            return View(customerDetail);
        }

        // GET: CustomerDetails/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Сustomers, "Id", "Id");
            ViewData["DetailId"] = new SelectList(_context.Details, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id");
            return View();
        }

        // POST: CustomerDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DetailId,CustomerId,BuyingDate,StatusId,Address")] CustomerDetail customerDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Сustomers, "Id", "Id", customerDetail.CustomerId);
            ViewData["DetailId"] = new SelectList(_context.Details, "Id", "Id", customerDetail.DetailId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", customerDetail.StatusId);
            return View(customerDetail);
        }

        // GET: CustomerDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerDetail = await _context.CustomerDetails.FindAsync(id);
            if (customerDetail == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Сustomers, "Id", "Id", customerDetail.CustomerId);
            ViewData["DetailId"] = new SelectList(_context.Details, "Id", "Id", customerDetail.DetailId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", customerDetail.StatusId);
            return View(customerDetail);
        }

        // POST: CustomerDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DetailId,CustomerId,BuyingDate,StatusId,Address")] CustomerDetail customerDetail)
        {
            if (id != customerDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerDetailExists(customerDetail.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Сustomers, "Id", "Id", customerDetail.CustomerId);
            ViewData["DetailId"] = new SelectList(_context.Details, "Id", "Id", customerDetail.DetailId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", customerDetail.StatusId);
            return View(customerDetail);
        }

        // GET: CustomerDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerDetail = await _context.CustomerDetails
                .Include(c => c.Customer)
                .Include(c => c.Detail)
                .Include(c => c.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerDetail == null)
            {
                return NotFound();
            }

            return View(customerDetail);
        }

        // POST: CustomerDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerDetail = await _context.CustomerDetails.FindAsync(id);
            if (customerDetail != null)
            {
                _context.CustomerDetails.Remove(customerDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerDetailExists(int id)
        {
            return _context.CustomerDetails.Any(e => e.Id == id);
        }
    }
}
