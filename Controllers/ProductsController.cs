using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Controllers
{
    [Authorize(Roles = "User")]
    public class ProductsController : Controller
    {
        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public ProductsController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /products
        [Route("products")]
        public async Task<IActionResult> Index(int p = 1)
        {
            var product = context.Products.OrderByDescending(x => x.Id);

            return View(await product.ToListAsync());
        }

        // GET /products/category
        public async Task<IActionResult> ProductsByCategory(string productDesc)
        {
            ProductType productType = await context.ProductTypes.Where(x => x.Description == productDesc).FirstOrDefaultAsync();
            if (productType == null) return RedirectToAction("Index");

            var products = context.Products.OrderByDescending(x => x.Id).Where(x => x.Id == productType.Id);

            ViewBag.Product = productType.Product;
            ViewBag.Description = productDesc;

            return View(await products.ToListAsync());
        }
    }
}