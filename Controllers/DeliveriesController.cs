using LiquefiedPetroleumGas.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Controllers
{
    public class DeliveriesController : Controller
    {

        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public DeliveriesController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /products
        [Route("deliveries")]
        public async Task<IActionResult> Index()
        {
            return View(await context.Orders.OrderBy(x => x.Id).ToListAsync());
        }
    }
}