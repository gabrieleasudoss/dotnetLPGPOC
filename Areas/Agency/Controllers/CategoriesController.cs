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
    public class CategoriesController : Controller
    {

        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public CategoriesController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /agency/categories
        public async Task<IActionResult> Index()
        {
            return View(await context.ProductTypes.OrderBy(x => x.Id).ToListAsync());
        }

        // GET /agency/categories/create
        public IActionResult Create() => View();

        // POST /agency/categories/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType productType)
        {
            Trace(productType);

            if (ModelState.IsValid)
            {
                productType.Id = Guid.NewGuid().ToString();
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                productType.Product = productType.Product;
                productType.Description = productType.Description;

                var description = await context.ProductTypes.FirstOrDefaultAsync(x => x.Description == productType.Description);
                if (description != null)
                {
                    ModelState.AddModelError("", "The product type already exists.");
                    return View(description);
                }

                context.Add(productType);
                await context.SaveChangesAsync();

                TempData["Success"] = "The product type has been added!";

                return RedirectToAction("Index");
            }

            return View(productType);
        }

        // GET /agency/categories/edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ProductType productType = await context.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // POST /agency/categories/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ProductType productType)
        {
            if (ModelState.IsValid)
            {
                Trace(productType);
                productType.Product = productType.Product;
                productType.Description = productType.Description;

                var productDescription = await context.ProductTypes.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Description == productType.Description);
                if (productDescription != null)
                {
                    ModelState.AddModelError("", "The product type already exists.");
                    return View(productType);
                }

                context.Update(productType);
                await context.SaveChangesAsync();

                TempData["Success"] = "The product type has been edited!";

                return RedirectToAction("Edit", new { id });
            }

            return View(productType);
        }

        // GET /agency/categories/delete/5
        public async Task<IActionResult> Delete(string id)
        {
            ProductType productType = await context.ProductTypes.FindAsync(id);

            if (productType == null)
            {
                TempData["Error"] = "The product type does not exist!";
            }
            else
            {
                context.ProductTypes.Remove(productType);
                await context.SaveChangesAsync();

                TempData["Success"] = "The product type has been deleted!";
            }

            return RedirectToAction("Index");
        }

        static ProductType Trace( ProductType productType)
        {
            productType.CreatedAt = DateTime.UtcNow;
            productType.CreatedBy = "Agency";
            productType.UpdatedAt = DateTime.UtcNow;
            productType.UpdatedBy = "Agency";
            return productType;
        }
    }
}
