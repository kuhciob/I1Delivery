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
    public class RestaurantDishRelationsController : Controller
    {
        private readonly I1Delivery_KursovaContext _context;

        public RestaurantDishRelationsController(I1Delivery_KursovaContext context)
        {
            _context = context;
        }

        // GET: RestaurantDishRelations
        //public async Task<IActionResult> Index()
        //{
        //    var i1Delivery_KursovaContext = _context.RestaurantDishRelation.Include(r => r.Dish)
        //        .Include(r => r.Restaurant)
        //        .ThenInclude(r => r.Location)
        //        .ThenInclude(r => r.Street)
        //        .ThenInclude(r => r.District)
        //        .ThenInclude(r => r.City)
        //        .OrderByDescending(m => m.CreatedDateTime);
        //    return View(await i1Delivery_KursovaContext.ToListAsync());
        //}
        public async Task<IActionResult> Index(int? restaurantID, string dishName)
        {
            var i1Delivery_KursovaContext = _context.RestaurantDishRelation.Include(r => r.Dish)
                .Include(r => r.Restaurant)
                .ThenInclude(r => r.Location)
                .ThenInclude(r => r.Street)
                .ThenInclude(r => r.District)
                .ThenInclude(r => r.City)
                 .Where(d => restaurantID != null ? d.RestaurantId == restaurantID : true)
                 .Where(d => string.IsNullOrEmpty(dishName) == false ? d.Dish.Title == dishName : true)
                .OrderByDescending(m => m.CreatedDateTime);

            var selectList = _context.Restaurant
               .Include(r => r.Location)
               .ThenInclude(c => c.Street)
               .ThenInclude(c => c.District)
               .ThenInclude(c => c.City)
               .Select(c => new
               {
                   RestaurantId = c.RestaurantId,
                   Descr = $"{c.Title} (id:{c.RestaurantId}, {c.Location.Street.District.City.City1})"
               });

            ViewData["RestaurantId"] = new SelectList(selectList, "RestaurantId", "Descr", restaurantID);

            return View(await i1Delivery_KursovaContext.ToListAsync());
        }

        // GET: RestaurantDishRelations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantDishRelation = await _context.RestaurantDishRelation
                .Include(r => r.Dish)
                .Include(r => r.Restaurant)
                .ThenInclude(r => r.Location)
                .ThenInclude(r => r.Street)
                .ThenInclude(r => r.District)
                .ThenInclude(r => r.City)
                .FirstOrDefaultAsync(m => m.RestaurantDishRelationId == id);
            if (restaurantDishRelation == null)
            {
                return NotFound();
            }

            return View(restaurantDishRelation);
        }

        // GET: RestaurantDishRelations/Create
        public IActionResult Create()
        {
            ViewData["DishId"] = new SelectList(_context.Dish, "DishId", "Title");
            var selectList = _context.Restaurant
                .Include(r => r.Location)
                .ThenInclude(c => c.Street)
                .ThenInclude(c => c.District)
                .ThenInclude(c => c.City)
                .Select(c => new
                {
                    RestaurantId = c.RestaurantId,
                    Descr = $"{c.Title} (id:{c.RestaurantId}, {c.Location.Street.District.City.City1})"
                });

            ViewData["RestaurantId"] = new SelectList(selectList, "RestaurantId", "Descr");
            return View();
        }

        // POST: RestaurantDishRelations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestaurantDishRelationId,RestaurantId,DishId,Price,CreatedDateTime,LastModifiedDateTime")] RestaurantDishRelation restaurantDishRelation)
        {
            if (ModelState.IsValid)
            {
                restaurantDishRelation.CreatedDateTime = DateTime.Now;
                restaurantDishRelation.LastModifiedDateTime = DateTime.Now;
                _context.Add(restaurantDishRelation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dish, "DishId", "Title", restaurantDishRelation.DishId);
            var selectList = _context.Restaurant
                .Include(r => r.Location)
                .ThenInclude(c => c.Street)
                .ThenInclude(c => c.District)
                .ThenInclude(c => c.City)
                .Select(c => new
                {
                    RestaurantId = c.RestaurantId,
                    Descr = $"{c.Title} (id:{c.RestaurantId}, {c.Location.Street.District.City.City1})"
                });

            ViewData["RestaurantId"] = new SelectList(selectList, "RestaurantId", "Descr", restaurantDishRelation.RestaurantId);
            return View(restaurantDishRelation);
        }

        // GET: RestaurantDishRelations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantDishRelation = await _context.RestaurantDishRelation
                .FindAsync(id);
            if (restaurantDishRelation == null)
            {
                return NotFound();
            }
            ViewData["DishId"] = new SelectList(_context.Dish, "DishId", "Title", restaurantDishRelation.DishId);

            var selectList = _context.Restaurant
                .Include(r=>r.Location)
                .ThenInclude(c => c.Street)
                .ThenInclude(c => c.District)
                .ThenInclude(c => c.City)
                .Select(c => new
                 {
                     RestaurantId = c.RestaurantId,
                     Descr = $"{c.Title} (id:{c.RestaurantId}, {c.Location.Street.District.City.City1})"
                 });

            ViewData["RestaurantId"] = new SelectList(selectList, "RestaurantId", "Descr", restaurantDishRelation.RestaurantId);
            return View(restaurantDishRelation);
        }

        // POST: RestaurantDishRelations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RestaurantDishRelationId,RestaurantId,DishId,Price,CreatedDateTime,LastModifiedDateTime")] RestaurantDishRelation restaurantDishRelation)
        {
            if (id != restaurantDishRelation.RestaurantDishRelationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (restaurantDishRelation.CreatedDateTime.Year < 2000)
                        restaurantDishRelation.CreatedDateTime = DateTime.Now;
                    restaurantDishRelation.LastModifiedDateTime = DateTime.Now;
                    _context.Update(restaurantDishRelation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantDishRelationExists(restaurantDishRelation.RestaurantDishRelationId))
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
            ViewData["DishId"] = new SelectList(_context.Dish, "DishId", "Title", restaurantDishRelation.DishId);
            var selectList = _context.Restaurant
                .Include(r => r.Location)
                .ThenInclude(c => c.Street)
                .ThenInclude(c => c.District)
                .ThenInclude(c => c.City)
                .Select(c => new
                {
                    RestaurantId = c.RestaurantId,
                    Descr = $"{c.Title} (id:{c.RestaurantId}, {c.Location.Street.District.City.City1})"
                });

            ViewData["RestaurantId"] = new SelectList(selectList, "RestaurantId", "Descr", restaurantDishRelation.RestaurantId);
            return View(restaurantDishRelation);
        }

        // GET: RestaurantDishRelations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantDishRelation = await _context.RestaurantDishRelation
                .Include(r => r.Dish)
                .Include(r => r.Restaurant)
                .FirstOrDefaultAsync(m => m.RestaurantDishRelationId == id);
            if (restaurantDishRelation == null)
            {
                return NotFound();
            }

            return View(restaurantDishRelation);
        }

        // POST: RestaurantDishRelations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurantDishRelation = await _context.RestaurantDishRelation.FindAsync(id);
            _context.RestaurantDishRelation.Remove(restaurantDishRelation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantDishRelationExists(int id)
        {
            return _context.RestaurantDishRelation.Any(e => e.RestaurantDishRelationId == id);
        }
    }
}
