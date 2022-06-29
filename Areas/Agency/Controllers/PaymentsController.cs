using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Areas.Agency.Controllers
{
    [Authorize(Roles = "Agency")]
    [Area("Agency")]
    public class PaymentsController : Controller
    {

        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public PaymentsController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /agency/payments
        public async Task<IActionResult> Index()
        {
            return View(await context.PaymentTypes.OrderBy(x => x.Id).ToListAsync());
        }

        // GET /agency/payments/create
        public IActionResult Create() => View();

        // POST /agency/payments/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentType paymentType)
        {
            Trace(paymentType);

            if (ModelState.IsValid)
            {
                paymentType.Id = Guid.NewGuid().ToString();
                paymentType.TypeName = paymentType.TypeName;

                var description = await context.ProductTypes.FirstOrDefaultAsync(x => x.Id == paymentType.Id);
                if (description != null)
                {
                    ModelState.AddModelError("", "The product type already exists.");
                    return View(description);
                }

                context.Add(paymentType);
                await context.SaveChangesAsync();

                TempData["Success"] = "The payment type has been added!";

                return Redirect("Index");
            }

            return View(paymentType);
        }

        // GET /agency/payments/edit/5
        public async Task<IActionResult> Edit(string id)
        {
            PaymentType paymentType = await context.PaymentTypes.FindAsync(id);
            if (paymentType == null)
            {
                return NotFound();
            }

            return View(paymentType);
        }

        // POST /agency/payments/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, PaymentType paymentType)
        {
            if (ModelState.IsValid)
            {
                Trace(paymentType);
                paymentType.TypeName = paymentType.TypeName;

                var productDescription = await context.PaymentTypes.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.TypeName == paymentType.TypeName);
                if (productDescription != null)
                {
                    ModelState.AddModelError("", "The payment type already exists.");
                    return View(paymentType);
                }

                context.Update(paymentType);
                await context.SaveChangesAsync();

                TempData["Success"] = "The payment type has been edited!";

                return RedirectToAction("Edit", new { id });
            }

            return View(paymentType);
        }

        // GET /agency/payments/delete/5
        public async Task<IActionResult> Delete(string id)
        {
            PaymentType paymentType = await context.PaymentTypes.FindAsync(id);

            if (paymentType == null)
            {
                TempData["Error"] = "The payment type does not exist!";
            }
            else
            {
                context.PaymentTypes.Remove(paymentType);
                await context.SaveChangesAsync();

                TempData["Success"] = "The payment type has been deleted!";
            }

            return RedirectToAction("Index");
        }

        static PaymentType Trace( PaymentType paymentType)
        {
            paymentType.CreatedAt = DateTime.UtcNow;
            paymentType.CreatedBy = "Agency";
            paymentType.UpdatedAt = DateTime.UtcNow;
            paymentType.UpdatedBy = "Agency";
            return paymentType;
        }
    }
}
