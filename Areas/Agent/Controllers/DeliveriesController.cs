using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Areas.Agent.Controllers
{
    [Authorize(Roles = "Agent")]
    [Area("Agent")]
    public class DeliveriesController : Controller
    {

        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public DeliveriesController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /agent/deliveries/Index
        [Route("deliveries/Index")]
        public IActionResult Index()
        {
            var x = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                             where user.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)
                             join
                             role in context.Roles on
                             userRole.RoleId equals role.Id
                             where role.Name == "Agent"
                             select new Users
                             {
                                 FirstName = user.FirstName
                             }).Single();
            var tables = new OrderViewModel
            {
                Orders = context.Orders.OrderBy(m => m.Id).Where(o => o.DeliveryAgent == AgentName.FirstName).ToList(),
                Customers = customer,
                OrderStatusCatalogs = status
            };
            return View(tables);
        }

        // GET /agent/deliveries/edit/5
        [Route("deliveries/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            Order order = await context.Orders.FindAsync(id);
            TempData["ID"] = id;
            if (order == null)
            {
                return NotFound();
            }
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
                OrderStatusCatalogs = status
            };
            ViewBag.OrderStatus = new SelectList(context.OrderStatusCatalogs.OrderBy(x => x.Id), "Id", "StatusName", status);
            return View(tables);
        }

        // POST /agency/deliveries/edit/5
        [Route("deliveries/Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                if (orderViewModel.Status == "null")
                {
                    return NotFound();
                }
                else
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

                return RedirectToAction("Index");
            }

            return View(orderViewModel);
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