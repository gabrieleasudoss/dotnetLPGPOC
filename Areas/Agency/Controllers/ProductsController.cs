using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Areas.Agency.Controllers
{
    [Authorize(Roles = "Agency")]
    [Area("Agency")]
    public class ProductsController : Controller
    {
        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public ProductsController(LiquefiedPetroleumGasLoginAndRegistrationContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
        }

        // GET /agency/products
        public async Task<IActionResult> Index()
        {
            var product = context.Products.OrderByDescending(x => x.Id);

            return View(await product.ToListAsync());
        }

        // GET /agency/products/details/5
        public async Task<IActionResult> Details(string id)
        {
            Product product = await context.Products.Include(x => x.ProductType).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET /agency/products/create
        public IActionResult Create()
        {
            ViewBag.ProductTypes = new SelectList(context.ProductTypes.OrderBy(x => x.Id), "Id", "Product");

            return View();
        }

        // POST /agency/products/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            Trace(product);
            ViewBag.ProductTypes = new SelectList(context.ProductTypes.OrderBy(x => x.Id), "Id", "Product");
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid().ToString();
                product.Description = product.Description;

                var productDescription = await context.Products.FirstOrDefaultAsync(x => x.Description == product.Description);
                if (productDescription != null)
                {
                    ModelState.AddModelError("", "The product already exists.");
                    return View(product);
                }

                context.Add(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "The product has been added!";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET /agency/products/edit/5
        public async Task<IActionResult> Edit(string id)
        {
            Product product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.ProductTypes = new SelectList(context.ProductTypes.OrderBy(x => x.Id), "Id", "Product", product.ProductTypeId);

            return View(product);
        }

        // POST /agency/products/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Product product)
        {
            Trace(product);
            ViewBag.ProductTypes = new SelectList(context.ProductTypes.OrderBy(x => x.Id), "Id", "Product", product.ProductTypeId);

            if (ModelState.IsValid)
            {
                product.Description = product.Description;

                var productDescription = await context.Products.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Description == product.Description);
                if (productDescription != null)
                {
                    ModelState.AddModelError("", "The product already exists.");
                    return View(product);
                }

                context.Update(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "The product has been edited!";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET /agency/products/delete/5
        public async Task<IActionResult> Delete(string id)
        {
            Product product = await context.Products.FindAsync(id);

            if (product == null)
            {
                TempData["Error"] = "The product does not exist!";
            }
            else
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "The product has been deleted!";
            }

            return RedirectToAction("Index");
        }

        static Product Trace(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;
            product.CreatedBy = "Agency";
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = "Agency";
            return product;
        }
    }
}
