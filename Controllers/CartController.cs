using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Mvc;

namespace LiquefiedPetroleumGas.Controllers
{
    public class CartController : Controller
    {
        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public CartController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /cart
        [Route("cart")]
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };
            //TempData["cartVM"] = cartVM;
            return View(cartVM);
        }

        // GET /cart/add/5
        public async Task<IActionResult> Add(string id)
        {
            Product product = await context.Products.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(x => x.ProductId == id.ToString()).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            } 
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return RedirectToAction("Index");

            return ViewComponent("SmallCart");
        }
        
        // GET /cart/decrease/5
        public IActionResult Decrease(string id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(x => x.ProductId == id.ToString()).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            } 
            else
            {
                cart.RemoveAll(x => x.ProductId == id.ToString());
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }
        
        // GET /cart/remove/5
        public IActionResult Remove(string id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(x => x.ProductId == id.ToString());

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }
        
        // GET /cart/clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            //return RedirectToAction("Page", "Pages");
            //return Redirect("/");
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString());

            return Ok();
        }
    }
}