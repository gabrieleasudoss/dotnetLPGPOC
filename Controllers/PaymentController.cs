using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Controllers
{
    public class PaymentController : Controller
    {
        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public PaymentController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /payment
        [Route("payment")]
        public IActionResult Payment()
        {
            ViewBag.PaymentType = new SelectList(context.PaymentTypes.OrderBy(x => x.Id), "Id", "TypeName");
            return View();
        }

        // POST /payment
        [Route("payment")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(Payment payment)
        {
            ViewBag.PaymentType = new SelectList(context.PaymentTypes.OrderBy(x => x.Id), "Id", "TypeName");
            Trace(payment);
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("PaymentTypeId", payment.PaymentTypeId);
                payment.Id = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("PaymentId", payment.Id);
                payment.Name = payment.Name;
                payment.CardNumber = payment.CardNumber;
                payment.ExpiryDate = payment.ExpiryDate;
                payment.Cvv = payment.Cvv;

                context.Add(payment);
                await context.SaveChangesAsync();

                TempData["Success"] = "The payment Done!";

                return RedirectToAction("Order", "Orders");
            }

            return View(payment);
        }

        Payment Trace(Payment payment)
        {
            payment.CreatedAt = DateTime.UtcNow;
            payment.CreatedBy = "Agency";
            payment.UpdatedAt = DateTime.UtcNow;
            payment.UpdatedBy = "Agency";
            return payment;
        }
    }
}
