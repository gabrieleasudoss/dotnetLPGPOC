using LiquefiedPetroleumGas.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace LiquefiedPetroleumGas.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LiquefiedPetroleumGasLoginAndRegistrationContext(serviceProvider.GetRequiredService<DbContextOptions<LiquefiedPetroleumGasLoginAndRegistrationContext>>()))
            {
                if (context.Pages.Any())
                {
                    return;
                }

                context.Pages.AddRange(
                    new Page
                    {
                        Title = "Home",
                        Description = "home",
                        Content = "home page",
                    },
                    new Page
                    {
                        Title = "About Us",
                        Description = "about-us",
                        Content = "about us page",
                    },
                    new Page
                    {
                        Title = "Services",
                        Description = "services",
                        Content = "services page",
                    },
                    new Page
                    {
                        Title = "Contact",
                        Description = "contact",
                        Content = "contact page",
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
