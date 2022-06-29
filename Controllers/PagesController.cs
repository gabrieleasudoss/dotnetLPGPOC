using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Controllers
{
    public class PagesController : Controller
    {
        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public PagesController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        // GET /
        public async Task<IActionResult> Page(string description)
        {
            if (description == null)
            {
                return View(await context.Pages.Where(x => x.Description == "home").FirstOrDefaultAsync());
            }

            Page page = await context.Pages.Where(x => x.Description == description).FirstOrDefaultAsync();

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
    }
}
