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
    public class SalaryPaymentsController : Controller
    {
        private readonly I1Delivery_KursovaContext _context;

        public SalaryPaymentsController(I1Delivery_KursovaContext context)
        {
            _context = context;
        }

        // GET: SalaryPayments
        public async Task<IActionResult> Index()
        {
            var i1Delivery_KursovaContext = _context.SalaryPayment.Include(s => s.Courier)
                .ThenInclude(s => s.CourierType)
                .OrderByDescending(s=>s.CreatedDateTime);
            return View(await i1Delivery_KursovaContext.ToListAsync());
        }

        // GET: SalaryPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryPayment = await _context.SalaryPayment
                .Include(s => s.Courier)
                .ThenInclude( s=>s.CourierType)
                .OrderByDescending(m => m.CreatedDateTime)
                .FirstOrDefaultAsync(m => m.SalaryPaymentId == id);
            if (salaryPayment == null)
            {
                return NotFound();
            }

            return View(salaryPayment);
        }

        // GET: SalaryPayments/Create
        public IActionResult Create()
        {
            var selectList = _context.Courier
                .Include(c => c.CourierType)
                .Select(c => new 
                { 
                    CourierId = c.CourierId,
                    Descr = $"{c.Name} {c.Surname} (id:{c.CourierId}; {c.CourierType.Type})" 
                });

            ViewData["CourierId"] = new SelectList(selectList, "CourierId", "Descr");
            return View();
        }

        // POST: SalaryPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalaryPaymentId,CourierId,StartPeriodDate,EndPeriodDate,DeliveriesCount,PaymentForDeliveries,Premium,FineAmt,PaymentAmt,CreatedDateTime,LastModifiedDateTime")] SalaryPayment salaryPayment)
        {
            if (ModelState.IsValid)
            {
                salaryPayment.CreatedDateTime = DateTime.Now;
                salaryPayment.LastModifiedDateTime = DateTime.Now;
                var deliveriesCount = _context
                    .Delivery
                    .Include(d=>d.Courier)
                    .ThenInclude(d=>d.CourierType)
                    .Where(
                    d => d.CourierId == salaryPayment.CourierId
                    && d.StartTime >= salaryPayment.StartPeriodDate
                    && d.StartTime <= salaryPayment.EndPeriodDate
                    && d.EndTime >= salaryPayment.StartPeriodDate
                    && d.EndTime <= salaryPayment.EndPeriodDate)
                    .Count();

                var courier = _context.Courier
                    .Include(c => c.CourierType)
                    .First(c=>c.CourierId == salaryPayment.CourierId);

                salaryPayment.DeliveriesCount = deliveriesCount;
                salaryPayment.PaymentForDeliveries = deliveriesCount * courier.CourierType.Rate;
                salaryPayment.PaymentAmt = salaryPayment.PaymentForDeliveries + 
                    salaryPayment.Premium.GetValueOrDefault(0) - 
                    salaryPayment.FineAmt.GetValueOrDefault(0);
                _context.Add(salaryPayment);
                await _context.SaveChangesAsync();
                return RedirectToRoute(new { controller = "SalaryPayments", action = "Details", id = salaryPayment.SalaryPaymentId });

            }
            ViewData["CourierId"] = new SelectList(_context.Courier, "CourierId", "Name", salaryPayment.CourierId);
            return View(salaryPayment);
        }

        // GET: SalaryPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryPayment = _context.SalaryPayment
                .Include(s => s.Courier)
                .ThenInclude(s => s.CourierType)
                .Where(s=>s.CourierId == id)
                .FirstOrDefault();
            if (salaryPayment == null)
            {
                return NotFound();
            }
            var selectList = _context.Courier
                .Include(c => c.CourierType)
                .Select(c => new
                {
                    CourierId = c.CourierId,
                    Descr = $"{c.Name} {c.Surname} (id:{c.CourierId}; {c.CourierType.Type})"
                });

            ViewData["CourierId"] = new SelectList(selectList, "CourierId", "Descr", salaryPayment.CourierId);
            //ViewData["CourierId"] = new SelectList(_context.Courier, "CourierId", "Name", salaryPayment.CourierId);
            return View(salaryPayment);
        }

        // POST: SalaryPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalaryPaymentId,CourierId,StartPeriodDate,EndPeriodDate,DeliveriesCount,PaymentForDeliveries,Premium,FineAmt,PaymentAmt,CreatedDateTime,LastModifiedDateTime")] SalaryPayment salaryPayment)
        {
            if (id != salaryPayment.SalaryPaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (salaryPayment.CreatedDateTime.Year < 2000)
                        salaryPayment.CreatedDateTime = DateTime.Now;
                    salaryPayment.LastModifiedDateTime = DateTime.Now;
                    _context.Update(salaryPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryPaymentExists(salaryPayment.SalaryPaymentId))
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
            ViewData["CourierId"] = new SelectList(_context.Courier, "CourierId", "Name", salaryPayment.CourierId);
            return View(salaryPayment);
        }

        // GET: SalaryPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryPayment = await _context.SalaryPayment
                .Include(s => s.Courier)
                .FirstOrDefaultAsync(m => m.SalaryPaymentId == id);
            if (salaryPayment == null)
            {
                return NotFound();
            }

            return View(salaryPayment);
        }

        // POST: SalaryPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salaryPayment = await _context.SalaryPayment.FindAsync(id);
            _context.SalaryPayment.Remove(salaryPayment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryPaymentExists(int id)
        {
            return _context.SalaryPayment.Any(e => e.SalaryPaymentId == id);
        }
    }
}
