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
    public class OrdersController : Controller
    {
        private readonly I1Delivery_KursovaContext _context;

        public OrdersController(I1Delivery_KursovaContext context)
        {
            _context = context;
        }

        // GET: Orders
        //[HttpGet("Orders")]
        //public async Task<IActionResult> Index()
        //{
        //    var i1Delivery_KursovaContext = _context.Order
        //        .Include(o => o.Customer)
        //        .Include(o => o.OrderLine)
        //        .ThenInclude(o => o.RestaurantDishRelation)
        //        .OrderBy(o=>o.CreatedDateTime);
        //    //.ToList();

        //    i1Delivery_KursovaContext.ToList().ForEach(o => o.TotalAmt = o.OrderLine.Sum(o => o.Quantity * (o.RestaurantDishRelation?.Price ?? 0)));
        //    return View(await i1Delivery_KursovaContext.ToListAsync());
        //}

        public async Task<IActionResult> Index(string DescrSearch, string PhoneSearch, OrderStatus? orderStatus)
        {
            var i1Delivery_KursovaContext =  _context.Order
                .Include(o => o.Customer)
                .Include(o => o.OrderLine)
                
                .ThenInclude(o => o.RestaurantDishRelation)
                .Where(o => string.IsNullOrEmpty(DescrSearch) == false? (string.IsNullOrEmpty(DescrSearch) == false ? o.Description.Contains(DescrSearch) : true): true)
                 .Where(o => string.IsNullOrEmpty(PhoneSearch) == false? o.Customer.Phone.Contains(PhoneSearch) : true)
                 .Where(o => orderStatus != null && o.Status != null? o.Status == orderStatus : true)
                 .OrderByDescending(o => o.CreatedDateTime)
                .ToList();

            var selectList = Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
               .Select(c => new
               {
                   orderStatus = c,
                   orderStatusDescr = c.GetDescription()
               });

            ViewData["orderStatuses"] = new SelectList(selectList, "orderStatus", "orderStatusDescr", orderStatus);

            i1Delivery_KursovaContext.ToList().ForEach(o => o.TotalAmt = o.OrderLine.Sum(o => o.Quantity * (o.RestaurantDishRelation?.Price ?? 0)));
            return View( i1Delivery_KursovaContext);
        }
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Customer)
                .Include(o => o.Invoice)
                .Include(o => o.OrderLine)
                .ThenInclude(o => o.RestaurantDishRelation)
                .ThenInclude(o => o.Dish)
                .ThenInclude(o => o.UnitOfMeasure)
                .Include(o => o.Delivery)
                .ThenInclude(o=>o.Location)
                .ThenInclude(o=>o.Street)
                .ThenInclude(o => o.District)
                .ThenInclude(o => o.City)
                .Include(o=>o.Delivery)
                .ThenInclude(o=>o.Courier)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }
            order.TotalAmt = order.OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Price);
            _context.Update(order);
            await _context.SaveChangesAsync();

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var selectList = _context.Customer
              .OrderBy(r => r.CustomerId)
              .Select(c => new
              {
                  CustomerId = c.CustomerId,
                  Descr = $"{c.Name}, {c.Phone} (id:{c.CustomerId})"
              });
            ViewData["CustomerId"] = new SelectList(selectList, "CustomerId", "Descr");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,TotalAmt,Description,CreatedDateTime,LastModifiedDateTime")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.CreatedDateTime = DateTime.Now;
                order.LastModifiedDateTime = DateTime.Now;
                order.Status = OrderStatus.Created;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToRoute(new { controller = "Orders", action = "Details", id = order.OrderId });

                //return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o=> o.Invoice)
                .Include(o => o.OrderLine)
                .ThenInclude(o => o.RestaurantDishRelation)
                .ThenInclude(o => o.Dish)
                .ThenInclude(o => o.UnitOfMeasure)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            order.TotalAmt = order.OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Price);
            var selectList = _context.Customer
              .OrderBy(r => r.CustomerId)
              .Select(c => new
              {
                  CustomerId = c.CustomerId,
                  Descr = $"{c.Name}, {c.Phone} (id:{c.CustomerId})"
              });
            ViewData["CustomerId"] = new SelectList(selectList, "CustomerId", "Descr", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,TotalAmt,Description,CreatedDateTime,LastModifiedDateTime")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (order.CreatedDateTime.Year < 2000)
                        order.CreatedDateTime = DateTime.Now;
                    order.LastModifiedDateTime = DateTime.Now;
                    order.TotalAmt = order
                        .OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Price);

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute(new { controller = "Orders", action = "Details", id = order.OrderId });

            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", order.CustomerId);
            return View(order);
        }
        [HttpPost("Orders/Process/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(int? id)
        {
            var order = _context.Order.Find(id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (order.CreatedDateTime.Year < 2000)
                        order.CreatedDateTime = DateTime.Now;
                    order.LastModifiedDateTime = DateTime.Now;
                    order.TotalAmt = order
                        .OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Price);
                    order.Status = OrderStatus.InProcess;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute(new { controller = "Orders", action = "Details", id = order.OrderId });

            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", order.CustomerId);
            return RedirectToRoute(new { controller = "Orders", action = "Details", id = order.OrderId });

        }
        [HttpPost("Orders/Cancel/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int? id)
        {
            var order = _context.Order.Find(id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (order.CreatedDateTime.Year < 2000)
                        order.CreatedDateTime = DateTime.Now;
                    order.LastModifiedDateTime = DateTime.Now;
                    order.TotalAmt = order
                        .OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Price);
                    order.Status = OrderStatus.Canceled;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute(new { controller = "Orders", action = "Details", id = order.OrderId });

            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", order.CustomerId);
            return RedirectToRoute(new { controller = "Orders", action = "Details", id = order.OrderId });

        }
        [HttpPost("Orders/Complete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int? id)
        {
            var order = _context.Order.Find(id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (order.CreatedDateTime.Year < 2000)
                        order.CreatedDateTime = DateTime.Now;
                    order.LastModifiedDateTime = DateTime.Now;
                    order.TotalAmt = order
                        .OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Price);
                    order.Status = OrderStatus.Completed;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute(new { controller = "Orders", action = "Details", id = order.OrderId });

            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", order.CustomerId);
            return RedirectToRoute(new { controller = "Orders", action = "Details", id = order.OrderId });


        }
        [HttpPost("Orders/Close/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(int? id)
        {
            var order = _context.Order.Find(id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (order.CreatedDateTime.Year < 2000)
                        order.CreatedDateTime = DateTime.Now;
                    order.LastModifiedDateTime = DateTime.Now;
                    order.TotalAmt = order
                        .OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Price);
                    order.Status = OrderStatus.Closed;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute(new { controller = "Orders", action = "Details", id = order.OrderId });

            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", order.CustomerId);
            return RedirectToRoute(new { controller = "Orders", action = "Details", id = order.OrderId });


        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
