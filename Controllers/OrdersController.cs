using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Controllers
{
    [Authorize(Roles = "User")]
    public class OrdersController : Controller
    {
        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public OrdersController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /orders/Index
        [Route("orders/Index")]
        public IActionResult Index()
        {
            var customer = (from o in context.Orders
                            join
                            u in context.Users on
                            o.CustomerId equals u.Id
                            orderby o.Id
                            select new Users
                            {
                                FirstName = u.FirstName,
                                PhoneNumber = u.PhoneNumber,
                                Email = u.Email
                            }).ToList();
            var status = (from o in context.Orders
                          join
                          os in context.OrderStatuses on
                          o.Id equals os.OrderId
                          join
                          osc in context.OrderStatusCatalogs on
                          os.StatusId equals osc.Id
                          orderby o.Id
                          select new OrderStatusCatalog
                          {
                              StatusName = osc.StatusName
                          }).ToList();
            var tables = new OrderViewModel
            {
                Orders = context.Orders.ToList(),
                Customers = customer,
                OrderStatusCatalogs = status
            };
            return View(tables);
        }

        // GET /orders/Order
        [Route("orders/Order")]
        public IActionResult Order()
        {
            return View();
        }

        // POST /orders
        [Route("orders/Order")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(Order order)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            Trace(order);
            OrderDetails orderDetails = new OrderDetails();
            if (ModelState.IsValid)
            {
                order.Id = Guid.NewGuid().ToString();
                order.OrderDate = DateTime.UtcNow;
                order.CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                order.PaymentTypeId = HttpContext.Session.GetString("PaymentTypeId");
                order.DeliveryDate = DateTime.UtcNow.AddDays(3);
                order.DeliveryAgent = "Unknown";
                order.DeliveryAddress = order.DeliveryAddress;
                order.ZipCode = order.ZipCode;
                
                context.Add(order);
                await context.SaveChangesAsync();

                foreach (var item in cart)
                {
                    Trace(orderDetails);
                    orderDetails.Id = Guid.NewGuid().ToString();
                    orderDetails.OrderId = order.Id;
                    orderDetails.ProductId = item.ProductId;
                    orderDetails.Quantity = item.Quantity.ToString();
                    orderDetails.UnitPrice = item.Price;
                    orderDetails.TotalPrice = item.Total;
                    orderDetails.DeliveryPrice = 100;
                    context.Add(orderDetails);
                    await context.SaveChangesAsync();
                }

                OrderStatus orderStatus = new OrderStatus();
                Trace(orderStatus);
                orderStatus.Id = Guid.NewGuid().ToString();
                orderStatus.OrderId = order.Id;
                orderStatus.StatusId = context.OrderStatusCatalogs.Where(s => s.StatusName == "ORDERED").Select(u => u.Id).FirstOrDefault();
                ViewBag.StatusId = context.OrderStatusCatalogs.Where(s => s.Id == orderStatus.StatusId).Select(u => u.StatusName).FirstOrDefault(); ;
                ViewBag.OrderStatus = new SelectList(context.OrderStatusCatalogs.OrderBy(x => x.Id), "Id", "StatusName");

                context.Add(orderStatus);
                await context.SaveChangesAsync();

                PaymentDetails paymentDetails = new PaymentDetails();
                Trace(paymentDetails);
                paymentDetails.Id = Guid.NewGuid().ToString();
                paymentDetails.OrderId = order.Id;
                paymentDetails.PaymentId = HttpContext.Session.GetString("PaymentId");
                paymentDetails.TotalCost = cart.Sum(x => x.Price * x.Quantity);

                context.Add(paymentDetails);
                await context.SaveChangesAsync();

                TempData["Success"] = "The Order is placed!";

                var inventory = await context.Inventories.FirstOrDefaultAsync(x => x.ProductId == orderDetails.ProductId);

                var updatedStock = int.Parse(inventory.InStock) - int.Parse(orderDetails.Quantity);
                inventory.InStock = updatedStock.ToString();
                context.Update(inventory);
                await context.SaveChangesAsync();
                TempData["Success"] = "The inventory has been edited!";

                HttpContext.Session.Remove("Cart");
                return Redirect("Index");
            }

            return View(order);
        }

        Order Trace(Order order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            order.UpdatedAt = DateTime.UtcNow;
            order.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return order;
        }

        OrderDetails Trace(OrderDetails orderDetails)
        {
            orderDetails.CreatedAt = DateTime.UtcNow;
            orderDetails.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            orderDetails.UpdatedAt = DateTime.UtcNow;
            orderDetails.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return orderDetails;
        }

        PaymentDetails Trace(PaymentDetails paymentDetails)
        {
            paymentDetails.CreatedAt = DateTime.UtcNow;
            paymentDetails.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            paymentDetails.UpdatedAt = DateTime.UtcNow;
            paymentDetails.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return paymentDetails;
        }

        OrderStatus Trace(OrderStatus orderStatus)
        {
            orderStatus.CreatedAt = DateTime.UtcNow;
            orderStatus.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            orderStatus.UpdatedAt = DateTime.UtcNow;
            orderStatus.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return orderStatus;
        }
    }
}
