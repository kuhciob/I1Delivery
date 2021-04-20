using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KURSOVA.Models;

namespace KURSOVA.Controllers
{
    public class CouriersController : Controller
    {
        private readonly I1Delivery_KursovaContext _context;

        public CouriersController(I1Delivery_KursovaContext context)
        {
            _context = context;
        }

        // GET: Couriers
        public async Task<IActionResult> Index()
        {
            var i1Delivery_KursovaContext = _context.Courier
                .Include(c => c.CourierType)
                .OrderByDescending(m => m.CreatedDateTime);
            return View(await i1Delivery_KursovaContext.ToListAsync());
        }

        // GET: Couriers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courier = await _context.Courier
                .Include(c => c.CourierType)
                .FirstOrDefaultAsync(m => m.CourierId == id);
            if (courier == null)
            {
                return NotFound();
            }

            return View(courier);
        }

        // GET: Couriers/Create
        public IActionResult Create()
        {
            ViewData["CourierTypeId"] = new SelectList(_context.CourierType, "CourierTypeId", "Type");
            return View();
        }

        // POST: Couriers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourierId,Name,Surname,Phone,CourierTypeId,CreatedDateTime,LastModifiedDateTime")] Courier courier)
        {
            if (ModelState.IsValid)
            {
                courier.CreatedDateTime = DateTime.Now;
                courier.LastModifiedDateTime = DateTime.Now;

                _context.Add(courier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourierTypeId"] = new SelectList(_context.CourierType, "CourierTypeId", "Type", courier.CourierTypeId);
            return View(courier);
        }

        // GET: Couriers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courier = await _context.Courier.FindAsync(id);
            if (courier == null)
            {
                return NotFound();
            }
            ViewData["CourierTypeId"] = new SelectList(_context.CourierType, "CourierTypeId", "Type", courier.CourierTypeId);
            return View(courier);
        }

        // POST: Couriers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourierId,Name,Surname,Phone,CourierTypeId,CreatedDateTime,LastModifiedDateTime")] Courier courier)
        {
            if (id != courier.CourierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (courier.CreatedDateTime.Year < 2000)
                        courier.CreatedDateTime = DateTime.Now;
                    courier.LastModifiedDateTime = DateTime.Now;
                    _context.Update(courier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourierExists(courier.CourierId))
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
            ViewData["CourierTypeId"] = new SelectList(_context.CourierType, "CourierTypeId", "Type", courier.CourierTypeId);
            return View(courier);
        }

        // GET: Couriers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courier = await _context.Courier
                .Include(c => c.CourierType)
                .FirstOrDefaultAsync(m => m.CourierId == id);
            if (courier == null)
            {
                return NotFound();
            }

            return View(courier);
        }

        // POST: Couriers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courier = await _context.Courier.FindAsync(id);
            _context.Courier.Remove(courier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourierExists(int id)
        {
            return _context.Courier.Any(e => e.CourierId == id);
        }
    }
}
