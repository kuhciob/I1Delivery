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
    public class OrderLinesController : Controller
    {
        private readonly I1Delivery_KursovaContext _context;

        public OrderLinesController(I1Delivery_KursovaContext context)
        {
            _context = context;
        }

        // GET: OrderLines
        //public async Task<IActionResult> Index()
        //{
        //    var i1Delivery_KursovaContext = _context.OrderLine
        //        .Include(o => o.Order)

        //        .Include(o => o.RestaurantDishRelation)
        //        .ThenInclude(o => o.Restaurant)
        //        .Include(o => o.RestaurantDishRelation)
        //        .ThenInclude(o => o.Dish)
        //        .ThenInclude(o => o.UnitOfMeasure)
        //        .OrderByDescending(m => m.CreatedDateTime);

        //    return View(await i1Delivery_KursovaContext.ToListAsync());
        //}
        public async Task<IActionResult> Index(int? restaurantID)
        {
            var i1Delivery_KursovaContext = _context.OrderLine
                .Include(o => o.Order)
                .Include(o => o.RestaurantDishRelation)
                .ThenInclude(o => o.Restaurant)
                .Include(o => o.RestaurantDishRelation)
                .ThenInclude(o => o.Dish)
                .ThenInclude(o => o.UnitOfMeasure)
                .Where(o => restaurantID != null? o.RestaurantDishRelation.RestaurantId == restaurantID : true)
                .Where(o => o.Order.Status == OrderStatus.Created)
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

        // GET: OrderLines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLine
                .Include(o => o.Order)
                .Include(o => o.RestaurantDishRelation)
                .ThenInclude(o=>o.Dish)
                .Include(o => o.RestaurantDishRelation)
                .ThenInclude(o => o.Restaurant)

                .FirstOrDefaultAsync(m => m.OrderLineId == id);
            if (orderLine == null)
            {
                return NotFound();
            }

            return View(orderLine);
        }

        // GET: OrderLines/Create
        [HttpGet()]
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Order
                .Include(o => o.Delivery)
                .Include(o => o.Invoice)
                .Where(o => o.Status == OrderStatus.Completed
                && o.Status != OrderStatus.Closed
                 && o.Status != OrderStatus.Canceled
                 && o.Delivery.Count == 0), "OrderId", "OrderId");

            var selectList = _context.RestaurantDishRelation
              .Include(r => r.Restaurant)
              .Include(d => d.Dish)
              .ThenInclude(d => d.UnitOfMeasure)
              .OrderBy(r => r.RestaurantId)
              .Select(c => new
              {
                  RestaurantDishRelationId = c.RestaurantDishRelationId,
                  Descr = $"({c.Restaurant.Title} id:{c.RestaurantId})," +
                  $" {c.Dish.Title} {c.Dish.Quantity}{c.Dish.UnitOfMeasure.UnitOfMeasure1} - " +
                  $"{c.Price}грн"
              });

            ViewData["RestaurantDishRelationId"] = new SelectList(selectList, "RestaurantDishRelationId", "Descr");
            return View();
        }

        // GET: OrderLines/Create
        [HttpGet("OrderLines/Create/{orderId}")]
        public IActionResult Create(int? orderId)
        {

            var selectList = _context.RestaurantDishRelation
               .Include(r=>r.Restaurant)
               .Include(d=>d.Dish)
               .ThenInclude(d=>d.UnitOfMeasure)
               .OrderBy(r=>r.RestaurantId)
               .Select(c => new
               {
                   RestaurantDishRelationId = c.RestaurantDishRelationId,
                   Descr = $"({c.Restaurant.Title} id:{c.RestaurantId})," +
                   $" {c.Dish.Title} {c.Dish.Quantity}{c.Dish.UnitOfMeasure.UnitOfMeasure1} - " +
                   $"{c.Price}грн"
               });
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", orderId.GetValueOrDefault());
            ViewData["SelectedOrderId"] = orderId.GetValueOrDefault();

            ViewData["RestaurantDishRelationId"] = new SelectList(selectList, "RestaurantDishRelationId", "Descr");

            return View();
        }

        // POST: OrderLines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderLineId,OrderId,RestaurantDishRelationId,Quantity,CreatedDateTime,LastModifiedDateTime")] OrderLine orderLine)
        {
            if (ModelState.IsValid)
            {
                orderLine.CreatedDateTime = DateTime.Now;
                orderLine.LastModifiedDateTime = DateTime.Now;
                var order = _context.Order.Find(orderLine.OrderId);
                order.TotalAmt = order.OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Price);
                _context.Update(order);
                _context.Add(orderLine);
                await _context.SaveChangesAsync();
                return RedirectToRoute(new { controller = "Orders", action = "Details", id = orderLine.OrderId });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", orderLine.OrderId);
            ViewData["RestaurantDishRelationId"] = new SelectList(_context.RestaurantDishRelation, "RestaurantDishRelationId", "RestaurantDishRelationId", orderLine.RestaurantDishRelationId);
            return RedirectToRoute(new { controller = "Details", action = "Order", id = orderLine.OrderId });

        }

        // GET: OrderLines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLine.FindAsync(id);
            if (orderLine == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", orderLine.OrderId);
            var selectList = _context.RestaurantDishRelation
              .Include(r => r.Restaurant)
              .Include(d => d.Dish)
              .ThenInclude(d => d.UnitOfMeasure)
              .OrderBy(r => r.RestaurantId)
              .Select(c => new
              {
                  RestaurantDishRelationId = c.RestaurantDishRelationId,
                  Descr = $"({c.Restaurant.Title} id:{c.RestaurantId})," +
                  $" {c.Dish.Title} {c.Dish.Quantity}{c.Dish.UnitOfMeasure.UnitOfMeasure1} - " +
                  $"{c.Price}грн"
              });

            ViewData["RestaurantDishRelationId"] = new SelectList(selectList, "RestaurantDishRelationId", "Descr", orderLine.RestaurantDishRelationId);
            return View(orderLine);
        }

        // POST: OrderLines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderLineId,OrderId,RestaurantDishRelationId,Quantity,CreatedDateTime,LastModifiedDateTime")] OrderLine orderLine)
        {
            if (id != orderLine.OrderLineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (orderLine.CreatedDateTime.Year < 2000)
                        orderLine.CreatedDateTime = DateTime.Now;
                    orderLine.LastModifiedDateTime = DateTime.Now;
                    var order = _context.Order.Find(orderLine.OrderId);
                    order.TotalAmt = order.OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Price);
                    _context.Update(order);
                    _context.Update(orderLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderLineExists(orderLine.OrderLineId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute(new { controller = "Orders", action = "Details", id = orderLine.OrderId });

            }
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", orderLine.OrderId);
            ViewData["RestaurantDishRelationId"] = new SelectList(_context.RestaurantDishRelation, "RestaurantDishRelationId", "RestaurantDishRelationId", orderLine.RestaurantDishRelationId);

            return View(orderLine);

        }

        // GET: OrderLines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLine
                .Include(o => o.Order)
                .Include(o => o.RestaurantDishRelation)
                .FirstOrDefaultAsync(m => m.OrderLineId == id);
            if (orderLine == null)
            {
                return NotFound();
            }

            return View(orderLine);
        }

        // POST: OrderLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderLine = await _context.OrderLine.FindAsync(id);
            _context.OrderLine.Remove(orderLine);
            var order = _context.Order.Find(orderLine.OrderId);
            order.TotalAmt = order.OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Price);
            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderLineExists(int id)
        {
            return _context.OrderLine.Any(e => e.OrderLineId == id);
        }
    }
}
