using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LiquefiedPetroleumGas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMemoryCache();
            services.AddSession(options =>
            {
                //options.IdleTimeout = TimeSpan.FromSeconds(2)
                //options.IdleTimeout = TimeSpan.FromDays(2)
            });

            services.AddRouting(options => options.LowercaseUrls = true);



            services.AddControllersWithViews();

            //IdentityUsers
            services.AddDbContext<LiquefiedPetroleumGasLoginAndRegistrationContext>(options => options.UseSqlServer
                (Configuration.GetConnectionString("LiquefiedPetroleumGasContext")));

            services.AddIdentity<Users, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<LiquefiedPetroleumGasLoginAndRegistrationContext>()
                .AddDefaultTokenProviders();
            //services.AddMvc().AddSessionStateTempDataProvider();
            //services.AddSession();
            //services.AddIdentityCore<Agents>()
            //.AddRoles<IdentityRole>()
            //.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Agents, IdentityRole>>()
            //.AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "pages",
                    "{slug?}",
                    defaults: new { controller = "Pages", action = "Page" });

                endpoints.MapControllerRoute(
                    "orders",
                    "{slug?}",
                    defaults: new { controller = "Orders", action = "Order" });

                endpoints.MapControllerRoute(
                    "deliveries",
                    "{slug?}",
                    defaults: new { controller = "Deliveries", action = "Delivery" });

                endpoints.MapControllerRoute(
                    "products",
                    "products/{Description}",
                    defaults: new { controller = "Products", action = "ProductsByCategory" });

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
