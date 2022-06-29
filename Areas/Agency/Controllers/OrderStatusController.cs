using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Areas.Agency.Controllers
{
    [Authorize(Roles = "Agency")]
    [Area("Agency")]
    public class OrderStatusController : Controller
    {
        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public OrderStatusController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /agency/orderstatus
        public async Task<IActionResult> Index()
        {
            IQueryable<OrderStatusCatalog> orderStatusCatalogs = from p in context.OrderStatusCatalogs select p;

            List<OrderStatusCatalog> orderStatusList = await orderStatusCatalogs.ToListAsync();

            return View(orderStatusList);
        }

        // GET /agency/orderstatus/create
        public IActionResult Create()
        {
            return View();
        }

        // POST /agency/orderstatus/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderStatusCatalog orderStatusCatalog)
        {
            Trace(orderStatusCatalog);
            if (ModelState.IsValid)
            {
                orderStatusCatalog.Id = Guid.NewGuid().ToString();
                orderStatusCatalog.StatusName = orderStatusCatalog.StatusName;
                context.Add(orderStatusCatalog);
                await context.SaveChangesAsync();
                TempData["Success"] = "The order status catalog has been added!";

                return RedirectToAction("Index");
            }

            return View(orderStatusCatalog);
        }

        // GET /agency/orderstatus/edit/5
        public async Task<IActionResult> Edit(string id)
        {
            OrderStatusCatalog orderStatusCatalog = await context.OrderStatusCatalogs.FindAsync(id);
            if (orderStatusCatalog == null)
            {
                return NotFound();
            }
            return View(orderStatusCatalog);
        }

        // POST /agency/orderstatus/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderStatusCatalog orderStatusCatalog)
        {
            Trace(orderStatusCatalog);
            if (ModelState.IsValid)
            {
                orderStatusCatalog.StatusName = orderStatusCatalog.StatusName;

                context.Update(orderStatusCatalog);
                await context.SaveChangesAsync();

                TempData["Success"] = "The order status catalog has been edited!";

                return RedirectToAction("Index");
            }

            return View(orderStatusCatalog);
        }

        // GET /agency/orderstatus/delete/5
        public async Task<IActionResult> Delete(string id)
        {
            OrderStatusCatalog orderStatusCatalog = await context.OrderStatusCatalogs.FindAsync(id);

            if (orderStatusCatalog == null)
            {
                TempData["Error"] = "The page does not exist!";
            }
            else
            {
                context.OrderStatusCatalogs.Remove(orderStatusCatalog);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been deleted!";
            }

            return RedirectToAction("Index");
        }

        static OrderStatusCatalog Trace(OrderStatusCatalog orderStatusCatalog)
        {
            orderStatusCatalog.CreatedAt = DateTime.UtcNow;
            orderStatusCatalog.CreatedBy = "Agency";
            orderStatusCatalog.UpdatedAt = DateTime.UtcNow;
            orderStatusCatalog.UpdatedBy = "Agency";
            return orderStatusCatalog;
        }
    }
}
