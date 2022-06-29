using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Areas.Agency.Controllers
{
    [Authorize(Roles = "Agency")]
    [Area("Agency")]
    public class OrdersController : Controller
    {

        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;
        public OrdersController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /agency/orders/Index
        [Route("/agency/orders/Index")]
        public IActionResult Index()
        {
            var customer = (from o in context.Orders
                            join
                            u in context.Users on
                            o.CustomerId equals u.Id
                            where u.Id == o.CustomerId
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

        // POST /agency/orders/Index
        [Route("agency/orders/Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string id)
        {
            List<Order> searchValue;
            var customer = (from o in context.Orders
                            join
                            u in context.Users on
                            o.CustomerId equals u.Id
                            where u.Id == o.CustomerId
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
            if (id == null)
            {
                searchValue = context.Orders.ToList();
            }
            else
            {
                searchValue = context.Orders.FromSqlRaw("EXECUTE dbo.SearchOrder {0}", id).ToList();
            }
            var tables = new OrderViewModel
            {
                Orders = searchValue,
                Customers = customer,
                OrderStatusCatalogs = status
            };
            return View(tables);
        }

        // GET /agency/orders/edit/5
        [Route("agency/orders/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            Order order = await context.Orders.FindAsync(id);
            TempData["ID"] = id;
            if (order == null)
            {
                return NotFound();
            }
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

            var AgentName = (from user in context.Users
                             join 
                             userRole in context.UserRoles on 
                             user.Id equals userRole.UserId
                             join 
                             role in context.Roles on 
                             userRole.RoleId equals role.Id
                             where role.Name == "Agent"
                             select new Users
                             {
                                FirstName = user.FirstName
                             }).ToList();


            var tables = new OrderViewModel
            {
                Orders = context.Orders.ToList(),
                OrderStatusCatalogs = status,
                Customers = customer,
                Agent = AgentName
            };
            ViewBag.AgentName = new SelectList(AgentName.Select(a => a.FirstName));
            ViewBag.OrderStatus = new SelectList(context.OrderStatusCatalogs.OrderBy(x => x.Id), "Id", "StatusName", status);
            return View(tables);
        }

        // POST /agency/orders/edit/5
        [Route("agency/orders/Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderViewModel orderViewModel)
        {
            var customer = (from o in context.Orders
                            join
                            u in context.Users on
                            o.CustomerId equals u.Id
                            where u.Id == o.CustomerId
                            orderby o.Id
                            select new Users
                            {
                                FirstName = u.FirstName,
                                PhoneNumber = u.PhoneNumber,
                                Email = u.Email
                            }).ToList();
            if (ModelState.IsValid)
            {
                if (orderViewModel.Status == "null" && orderViewModel.AgentName == "null")
                {
                    return NotFound();
                }
                else if(orderViewModel.Status != "null")
                {
                    OrderStatus orderStatus = new OrderStatus();
                    Trace(orderStatus);
                    if (TempData.ContainsKey("ID"))
                    {
                        orderStatus.OrderId = TempData["ID"].ToString();
                        orderStatus.Id = context.OrderStatuses.Where(o => o.OrderId == orderStatus.OrderId).Select(od => od.Id).Single();
                    }
                    orderStatus.StatusId = orderViewModel.Status;

                    context.Update(orderStatus);
                    await context.SaveChangesAsync();

                    TempData["Success"] = "The order status has been edited!";

                }
                else
                {
                    if (TempData.ContainsKey("ID"))
                    {
                        var x = context.Orders.Where(o => o.Id == TempData["ID"].ToString()).Single();
                        x.DeliveryAgent = orderViewModel.AgentName;
                        context.Orders.Update(x);
                    }
                    await context.SaveChangesAsync();

                    TempData["Success"] = "The order agent has been edited!";

                }
                return RedirectToAction("Index", "Orders");
            }
            var tables = new OrderViewModel
            {
                Customers = customer
            };
            return View(orderViewModel);
        }

        // GET /agency/orders/details/5
        [Route("agency/orders/Details")]
        public IActionResult Details(string id)
        {

            var orderDetails = context.OrderDetails.OrderBy(x => x.Id).Where(w => w.OrderId == id).ToList();
            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        OrderStatus Trace(OrderStatus orderStatus)
        {
            orderStatus.CreatedAt = DateTime.UtcNow;
            orderStatus.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            orderStatus.UpdatedAt = DateTime.UtcNow;
            orderStatus.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return orderStatus;
        }
        
        Order Trace(Order order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            order.UpdatedAt = DateTime.UtcNow;
            order.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return order;
        }
    }
}