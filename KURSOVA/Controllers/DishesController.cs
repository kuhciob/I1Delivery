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
    public class DishesController : Controller
    {
        private readonly I1Delivery_KursovaContext _context;

        public DishesController(I1Delivery_KursovaContext context)
        {
            _context = context;
        }

        // GET: Dishes
        //public async Task<IActionResult> Index()
        //{
        //    var i1Delivery_KursovaContext = _context.Dish.Include(d => d.DishType).Include(d => d.UnitOfMeasure)
        //        .OrderByDescending(m => m.CreatedDateTime);
        //    ViewData["DishTypeId"] = new SelectList(_context.DishType, "DishTypeId", "DishType1", dish.DishTypeId);

        //    return View(await i1Delivery_KursovaContext.ToListAsync());
        //}
        public async Task<IActionResult> Index(int? dishTypeId, string dishName)
        {
            var i1Delivery_KursovaContext = _context.Dish
                .Include(d => d.DishType)
                .Include(d => d.UnitOfMeasure)
                .Where(d=> dishTypeId != null ? d.DishTypeId == dishTypeId : true)
                 .Where(d => string.IsNullOrEmpty(dishName) == false ? d.Title == dishName : true)

                .OrderByDescending(m => m.CreatedDateTime);
            ViewData["DishTypeId"] = new SelectList(_context.DishType, "DishTypeId", "DishType1", dishTypeId);

            return View(await i1Delivery_KursovaContext.ToListAsync());
        }
        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.DishType)
                .Include(d => d.UnitOfMeasure)
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            ViewData["DishTypeId"] = new SelectList(_context.DishType, "DishTypeId", "DishType1");
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasure, "UnitOfMeasureId", "UnitOfMeasure1");
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishId,Title,Quantity,UnitOfMeasureId,DishTypeId,Description,CreatedDateTime,LastModifiedDateTime,Cost")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                dish.CreatedDateTime = DateTime.Now;
                dish.LastModifiedDateTime = DateTime.Now;
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishTypeId"] = new SelectList(_context.DishType, "DishTypeId", "DishType1", dish.DishTypeId);
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasure, "UnitOfMeasureId", "UnitOfMeasure1", dish.UnitOfMeasureId);
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["DishTypeId"] = new SelectList(_context.DishType, "DishTypeId", "DishType1", dish.DishTypeId);
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasure, "UnitOfMeasureId", "UnitOfMeasure1", dish.UnitOfMeasureId);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishId,Title,Quantity,UnitOfMeasureId,DishTypeId,Description,CreatedDateTime,LastModifiedDateTime,Cost")] Dish dish)
        {
            if (id != dish.DishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (dish.CreatedDateTime.Year < 2000)
                        dish.CreatedDateTime = DateTime.Now;
                    dish.LastModifiedDateTime = DateTime.Now;
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.DishId))
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
            ViewData["DishTypeId"] = new SelectList(_context.DishType, "DishTypeId", "DishType1", dish.DishTypeId);
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasure, "UnitOfMeasureId", "UnitOfMeasure1", dish.UnitOfMeasureId);
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.DishType)
                .Include(d => d.UnitOfMeasure)
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.DishId == id);
        }
    }
}
