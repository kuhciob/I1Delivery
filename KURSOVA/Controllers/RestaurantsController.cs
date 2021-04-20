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
    public class RestaurantsController : Controller
    {
        private readonly I1Delivery_KursovaContext _context;

        public RestaurantsController(I1Delivery_KursovaContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            var i1Delivery_KursovaContext = _context.Restaurant
                .Include(r => r.Location)
                .ThenInclude(r => r.Street)
                .ThenInclude(r => r.District)
                .ThenInclude(r => r.City)
                .OrderByDescending(m => m.CreatedDateTime);
            return View(await i1Delivery_KursovaContext.ToListAsync());
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(r => r.Location)
                .ThenInclude(r => r.Street)
                .ThenInclude(r => r.District)
                .ThenInclude(r => r.City)
                .FirstOrDefaultAsync(m => m.RestaurantId == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            var selectList = _context.Location
                .Include(c => c.Street)
                .ThenInclude(c => c.District)
                .ThenInclude(c => c.City)
                .Select(c => new
                {
                    LocationId = c.LocationId,
                    Descr = $"{c.Street.District.City.City1}, {c.Street.District.District1}, " +
                    $"{c.Street.Street1}, {c.BuildingNbr}/{c.Room})"
                });
            ViewData["LocationId"] = new SelectList(selectList, "LocationId", "Descr");
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestaurantId,Title,Phone,LocationId,CreatedDateTime,LastModifiedDateTime")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                restaurant.CreatedDateTime = DateTime.Now;
                restaurant.LastModifiedDateTime = DateTime.Now;
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var selectList = _context.Location
                 .Include(c => c.Street)
                 .ThenInclude(c => c.District)
                 .ThenInclude(c => c.City)
                 .Select(c => new
                 {
                     LocationId = c.LocationId,
                     Descr = $"{c.Street.District.City.City1}, {c.Street.District.District1}, " +
                     $"{c.Street.Street1}, {c.BuildingNbr}/{c.Room})"
                 });
            ViewData["LocationId"] = new SelectList(selectList, "LocationId", "Descr", restaurant.LocationId);
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            var selectList = _context.Location
                .Include(c => c.Street)
                .ThenInclude(c => c.District)
                .ThenInclude(c => c.City)
                .Select(c => new
                {
                    LocationId = c.LocationId,
                    Descr = $"{c.Street.District.City.City1}, {c.Street.District.District1}, " +
                    $"{c.Street.Street1}, {c.BuildingNbr}/{c.Room})"
                });
            ViewData["LocationId"] = new SelectList(selectList, "LocationId", "Descr", restaurant.LocationId);
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RestaurantId,Title,Phone,LocationId,CreatedDateTime,LastModifiedDateTime")] Restaurant restaurant)
        {
            if (id != restaurant.RestaurantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (restaurant.CreatedDateTime.Year < 2000)
                        restaurant.CreatedDateTime = DateTime.Now;
                    restaurant.LastModifiedDateTime = DateTime.Now;
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.RestaurantId))
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
            var selectList = _context.Location
                .Include(c => c.Street)
                .ThenInclude(c => c.District)
                .ThenInclude(c => c.City)
                .Select(c => new
                {
                    LocationId = c.LocationId,
                    Descr = $"{c.Street.District.City.City1}, {c.Street.District.District1}, " +
                    $"{c.Street.Street1}, {c.BuildingNbr}/{c.Room})"
                });
            ViewData["LocationId"] = new SelectList(selectList, "LocationId", "Descr", restaurant.LocationId);
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(r => r.Location)
                .ThenInclude(d => d.Street)
                .ThenInclude(d => d.District)
                .ThenInclude(d => d.City)

                .FirstOrDefaultAsync(m => m.RestaurantId == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurant.FindAsync(id);
            _context.Restaurant.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurant.Any(e => e.RestaurantId == id);
        }
    }
}
