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
    public class InvoicesController : Controller
    {
        private readonly I1Delivery_KursovaContext _context;

        public InvoicesController(I1Delivery_KursovaContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var i1Delivery_KursovaContext = _context.Invoice.Include(i => i.Order).OrderByDescending(m => m.CreatedDateTime);
            return View(await i1Delivery_KursovaContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.Order)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        [HttpGet()]
        public IActionResult Create()
        {

            ViewData["OrderId"] = new SelectList(_context.Order
                .Include(o=>o.Delivery)
                .Include(o => o.Invoice)
                .Where(o => o.Status == OrderStatus.Completed
                && o.Status != OrderStatus.Closed
                 && o.Status != OrderStatus.Canceled
                 && o.Delivery.Count == 0), "OrderId", "OrderId");
            return View();
        }
        [HttpGet("Invoices/Create/{orderId}")]
        public IActionResult Create(int? orderId)
        {
            var order = _context.Order
                .Include(o => o.Delivery)
                .ThenInclude(d => d.Courier)
                .ThenInclude( d => d.CourierType)
                .Include(o => o.OrderLine)
                .ThenInclude(o => o.RestaurantDishRelation)
                .ThenInclude(o => o.Dish)
                .First(o => o.OrderId == orderId);

            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", orderId);
            ViewData["OrderTotal"] = _context.Order.Find(orderId).TotalAmt;
            Invoice invoice = new Invoice();
            invoice.OrderId = orderId.GetValueOrDefault();
            invoice.TotalAmt = order.OrderLine.Sum(o=>o.Quantity * o.RestaurantDishRelation.Price);
            var delivery = order.Delivery.OrderByDescending(o => o.CreatedDateTime).FirstOrDefault();
            if(delivery != null)
            {
                invoice.DeliveryPrice = delivery.Courier.CourierType.Rate;
            }
            return View(invoice);
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceId,OrderId,DeliveryPrice,TotalAmt,Discount,DateTime,CreatedDateTime,LastModifiedDateTime")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                var order = _context.Order
                    .Include(o => o.OrderLine)
                    .ThenInclude(o => o.RestaurantDishRelation)
                    .Include(o => o.Delivery)
                    .ThenInclude(o => o.Courier)
                    .ThenInclude(o => o.CourierType)
                    .First(o => o.OrderId == invoice.OrderId);
 

                invoice.CreatedDateTime = DateTime.Now;
                invoice.LastModifiedDateTime = DateTime.Now;
                invoice.TotalAmt = order.OrderLine.Sum(o => o.Quantity * o.RestaurantDishRelation.Price);
                invoice.TotalAmt += order.Delivery.FirstOrDefault()?.Courier.CourierType.Rate ?? 0;
                invoice.TotalAmt = invoice.TotalAmt * (100m - invoice.Discount)/100.0m;
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToRoute(new { controller = "Invoices", action = "Details", id = invoice.InvoiceId });

            }
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", invoice.OrderId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", invoice.OrderId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceId,OrderId,DeliveryPrice,TotalAmt,Discount,DateTime,CreatedDateTime,LastModifiedDateTime")] Invoice invoice)
        {
            if (id != invoice.InvoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (invoice.CreatedDateTime.Year < 2000)
                        invoice.CreatedDateTime = DateTime.Now;
                    invoice.LastModifiedDateTime = DateTime.Now;
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceId))
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
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", invoice.OrderId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.Order)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.InvoiceId == id);
        }
    }
}
