using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Infrastructure
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public MainMenuViewComponent(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();
            return View(pages);
        }

        private Task<List<Page>> GetPagesAsync()
        {
            return context.Pages.OrderBy(x => x.Id).ToListAsync();
        }
    }
}
