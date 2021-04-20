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
    public class DeliveriesController : Controller
    {
        private readonly I1Delivery_KursovaContext _context;

        public DeliveriesController(I1Delivery_KursovaContext context)
        {
            _context = context;
        }

        // GET: Deliveries
        public async Task<IActionResult> Index(DeliveryStatus? deliveryStatus)
        {
            var i1Delivery_KursovaContext = _context.Delivery.
                Include(d => d.Courier)
                .Include(d => d.Location)
                .ThenInclude(d => d.Street)
                .ThenInclude(d => d.District)
                .ThenInclude(d => d.City)
                .Include(d => d.Order)
                 .Where(o => deliveryStatus != null && o.Status != null ? o.Status == deliveryStatus : true)

                .OrderByDescending(m => m.CreatedDateTime);

            var selectList = Enum.GetValues(typeof(DeliveryStatus))
                .Cast<OrderStatus>()
               .Select(c => new
               {
                   deliveryStatus = c,
                   deliveryStatusDescr = c.GetDescription()
               });

            ViewData["deliveryStatuses"] = new SelectList(selectList, "deliveryStatus", "deliveryStatusDescr", deliveryStatus);
            return View(await i1Delivery_KursovaContext.ToListAsync());
        }

        // GET: Deliveries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Delivery
                .Include(d => d.Courier)
                .Include(d => d.Location)
                .ThenInclude(d => d.Street)
                .ThenInclude(d => d.District)
                .ThenInclude(d => d.City)
                .Include(d => d.Order)
                .ThenInclude(d => d.Invoice)
                .FirstOrDefaultAsync(m => m.DeliveryId == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // GET: Deliveries/Create
        [HttpGet()]
        public IActionResult Create()
        {
            //var lacationDrop = new SelectList(_context.Location, "LocationId", "BuildingNbr");

            var selectListCouriers = _context.Courier
                .Include(c => c.CourierType)
                .Select(c => new
                {
                    CourierId = c.CourierId,
                    Descr = $"{c.Name} {c.Surname} (id:{c.CourierId}; {c.CourierType.Type})"
                });

            ViewData["CourierId"] = new SelectList(selectListCouriers, "CourierId", "Descr");
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
            ViewData["OrderId"] = new SelectList(_context.Order
                .Include(o => o.Delivery)
                .Include(o => o.Invoice)
                .Where(o=>o.Status == OrderStatus.Completed 
                && o.Status != OrderStatus.Closed
                 && o.Status != OrderStatus.Canceled
                 && o.Delivery.Count == 0),
                "OrderId", "OrderId");
            return View();
        }
        [HttpGet("Deliveries/Create/{deliveryOrderId}")]
        public IActionResult Create(int? deliveryOrderId)
        {
            //var lacationDrop = new SelectList(_context.Location, "LocationId", "BuildingNbr");
            var order = _context.Order
                .Include(o=>o.Delivery)
                .Include(o => o.OrderLine)
                .ThenInclude(o => o.RestaurantDishRelation)
                .ThenInclude(o => o.Dish)
                .First(o => o.OrderId == deliveryOrderId);

            if(order.Delivery.Count > 0)
            {
                return RedirectToRoute(new { controller = "Deliveries", action = "Details", id = order.Delivery.FirstOrDefault()?.DeliveryId });
            }
            var selectListCouriers = _context.Courier
                .Include(c => c.CourierType)
                .Select(c => new
                {
                    CourierId = c.CourierId,
                    Descr = $"{c.Name} {c.Surname} (id:{c.CourierId}; {c.CourierType.Type})"
                });

            ViewData["CourierId"] = new SelectList(selectListCouriers, "CourierId", "Descr");
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
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", deliveryOrderId);
            Delivery delivery = new Delivery();
            delivery.Status = DeliveryStatus.Created;
            delivery.OrderId = deliveryOrderId.GetValueOrDefault();
            delivery.Weight = order.OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Dish.Quantity);
            delivery.CreatedDateTime = DateTime.Now;
            delivery.LastModifiedDateTime = DateTime.Now;
            delivery.StartTime = DateTime.Now;
            delivery.EndTime = DateTime.Now.AddMinutes(60);

            return View(delivery);
        }
        // POST: Deliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeliveryId,OrderId,CourierId,LocationId,Description,StartTime,EndTime,CreatedDateTime,LastModifiedDateTime,Weight")] Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                delivery.CreatedDateTime = DateTime.Now;
                delivery.LastModifiedDateTime = DateTime.Now;
                delivery.Status = DeliveryStatus.Created;
                _context.Add(delivery);
                await _context.SaveChangesAsync();
                return RedirectToRoute(new { controller = "Deliveries", action = "Details", id = delivery.DeliveryId });

            }

            ViewData["CourierId"] = new SelectList(_context.Courier, "CourierId", "Name", delivery.CourierId);
            var selectList = _context.Location
                .Include(c => c.Street)
                .ThenInclude(c => c.District)
                .ThenInclude(c => c.City)
                .Select(c => new
                {
                    LocationId = c.LocationId,
                    Descr = $"{c.Street.District.City.City1}, {c.Street.District.District1}, " +
                    $"{c.Street.Street1}, {c.Street.Street1}, {c.BuildingNbr}/{c.Room})"
                });
            ViewData["LocationId"] = new SelectList(selectList, "LocationId", "Descr", delivery.LocationId);
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", delivery.OrderId);
            return View(delivery);
        }

        // GET: Deliveries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Delivery.FindAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }
            var selectListCouriers = _context.Courier
                .Include(c => c.CourierType)
                .Select(c => new
                {
                    CourierId = c.CourierId,
                    Descr = $"{c.Name} {c.Surname} (id:{c.CourierId}; {c.CourierType.Type})"
                });

            ViewData["CourierId"] = new SelectList(selectListCouriers, "CourierId", "Descr", delivery.CourierId);
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
            ViewData["LocationId"] = new SelectList(selectList, "LocationId", "Descr", delivery.LocationId);
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", delivery.OrderId);
            return View(delivery);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeliveryId,OrderId,CourierId,LocationId,Description,StartTime,EndTime,CreatedDateTime,LastModifiedDateTime,Weight")] Delivery delivery)
        {
            if (id != delivery.DeliveryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (delivery.CreatedDateTime.Year < 2000)
                        delivery.CreatedDateTime = DateTime.Now;
                    delivery.LastModifiedDateTime = DateTime.Now;
                    _context.Update(delivery);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryExists(delivery.DeliveryId))
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
            ViewData["CourierId"] = new SelectList(_context.Courier, "CourierId", "Name", delivery.CourierId);
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
            ViewData["LocationId"] = new SelectList(selectList, "LocationId", "Descr", delivery.LocationId);
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", delivery.OrderId);
            return View(delivery);
        }

        // GET: Deliveries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Delivery
                .Include(d => d.Courier)
                .Include(d => d.Location)
                .Include(d => d.Order)
                .FirstOrDefaultAsync(m => m.DeliveryId == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var delivery = await _context.Delivery.FindAsync(id);
            _context.Delivery.Remove(delivery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryExists(int id)
        {
            return _context.Delivery.Any(e => e.DeliveryId == id);
        }

        [HttpPost("Deliveries/Ship/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ship(int? id)
        {
            var delivery = _context.Delivery.Find(id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (delivery.CreatedDateTime.Year < 2000)
                        delivery.CreatedDateTime = DateTime.Now;
                    delivery.LastModifiedDateTime = DateTime.Now;

                    delivery.Status = DeliveryStatus.Shipping;
                    var order = _context.Order.Find(delivery.OrderId);
                    order.Status = OrderStatus.Shipping;

                    _context.Update(delivery);
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryExists(delivery.DeliveryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute(new { controller = "Deliveries", action = "Details", id = delivery.DeliveryId });

            }
            return RedirectToRoute(new { controller = "Deliveries", action = "Details", id = delivery.DeliveryId });

        }
        [HttpPost("Deliveries/Complete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int? id)
        {
            var delivery = _context.Delivery.Find(id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (delivery.CreatedDateTime.Year < 2000)
                        delivery.CreatedDateTime = DateTime.Now;
                    delivery.LastModifiedDateTime = DateTime.Now;

                    delivery.Status = DeliveryStatus.Done;

                    var order = _context.Order.Find(delivery.OrderId);
                    order.Status = OrderStatus.Closed;

                    _context.Update(delivery);
                    _context.Update(order);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryExists(delivery.DeliveryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute(new { controller = "Deliveries", action = "Details", id = delivery.DeliveryId });

            }
            return RedirectToRoute(new { controller = "Deliveries", action = "Details", id = delivery.DeliveryId });

        }
        [HttpPost("Deliveries/Cancel/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int? id)
        {
            var delivery = _context.Delivery.Find(id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (delivery.CreatedDateTime.Year < 2000)
                        delivery.CreatedDateTime = DateTime.Now;
                    delivery.LastModifiedDateTime = DateTime.Now;

                    delivery.Status = DeliveryStatus.Canceled;
                    _context.Update(delivery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryExists(delivery.DeliveryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute(new { controller = "Deliveries", action = "Details", id = delivery.DeliveryId });

            }
            return RedirectToRoute(new { controller = "Deliveries", action = "Details", id = delivery.DeliveryId });

        }
    }
}
