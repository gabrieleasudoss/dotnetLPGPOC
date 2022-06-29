using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Areas.Agency.Controllers
{
    [Authorize(Roles = "Agency")]
    [Area("Agency")]
    public class InventoriesController : Controller
    {
        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public InventoriesController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /agency/inventories
        public async Task<IActionResult> Index()
        {
            IQueryable<Inventory> inventories = from p in context.Inventories select p;

            List<Inventory> inventoryList = await inventories.ToListAsync();

            return View(inventoryList);
        }

        // GET /agency/inventories/create
        public IActionResult Create()
        {
            ViewBag.Products = new SelectList(context.Products.OrderBy(x => x.Id), "Id", "Name");

            return View();
        }

        //GET /agency/inventories/details/5
        public async Task<IActionResult> Details(string id)
        {
            Inventory inventory = await context.Inventories.FirstOrDefaultAsync(x => x.ProductId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST /agency/inventories/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inventory inventory)
        {
            Trace(inventory);
            ViewBag.Products = new SelectList(context.Products.OrderBy(x => x.Id), "Id", "Name");
            if (ModelState.IsValid)
            {
                var productDescription = await context.Inventories.FirstOrDefaultAsync(x => x.ProductId == inventory.ProductId);
                if (productDescription != null)
                {
                    ModelState.AddModelError("", "The product already exists.");
                    return View(inventory);
                }

                inventory.Id = Guid.NewGuid().ToString();
                inventory.InStock = inventory.InStock;
                context.Add(inventory);
                await context.SaveChangesAsync();
                TempData["Success"] = "The inventory has been added!";

                return RedirectToAction("Index");
            }

            return View(inventory);
        }

        // GET /agency/inventories/edit/5
        public async Task<IActionResult> Edit(string id)
        {
            Inventory inventory = await context.Inventories.FindAsync(id);
            TempData["Stock"] = inventory.InStock;
            if (inventory == null)
            {
                return NotFound();
            }
            ViewBag.Products = new SelectList(context.Products.OrderBy(x => x.Id), "Id", "Name", inventory.ProductId);
            return View(inventory);
        }

        // POST /agency/inventories/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Inventory inventory)
        {
            Trace(inventory);
            if (ModelState.IsValid)
            {
                var stockNow = int.Parse(TempData["Stock"].ToString()) - int.Parse(inventory.InStock);
                inventory.InStock = inventory.InStock;
                inventory.InStock = stockNow.ToString();

                context.Update(inventory);
                await context.SaveChangesAsync();

                TempData["Success"] = "The inventory has been edited!";

                return RedirectToAction("Index");
            }

            return View(inventory);
        }

        // GET /agency/inventories/delete/5
        public async Task<IActionResult> Delete(string id)
        {
            Inventory inventory = await context.Inventories.FindAsync(id);

            if (inventory == null)
            {
                TempData["Error"] = "The inventory does not exist!";
            }
            else
            {
                context.Inventories.Remove(inventory);
                await context.SaveChangesAsync();

                TempData["Success"] = "The inventory has been deleted!";
            }

            return RedirectToAction("Index");
        }

        static Inventory Trace(Inventory inventory)
        {
            inventory.CreatedAt = DateTime.UtcNow;
            inventory.CreatedBy = "Agency";
            inventory.UpdatedAt = DateTime.UtcNow;
            inventory.UpdatedBy = "Agency";
            return inventory;
        }
    }
}
