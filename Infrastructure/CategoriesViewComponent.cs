using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Infrastructure
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;

        public CategoriesViewComponent(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await GetCategoriesAsync();
            return View(categories);
        }

        private Task<List<ProductType>> GetCategoriesAsync()
        {
            return context.ProductTypes.OrderBy(x => x.Id).ToListAsync();
        }
    }
}
