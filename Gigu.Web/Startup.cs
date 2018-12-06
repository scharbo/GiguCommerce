using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Gigu.Web.DataContext;
using Gigu.Web.Models;
using Gigu.Web.Paypal;
using Gigu.Web.Services.Infrastructure;
using Gigu.Web.Services.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Gigu.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "App_Data";
            });

            services.AddMvc()
         .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
         .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            /*Adler*/
            services.AddOptions();
            services.Configure<PaypalSettings>(_config.GetSection("PaypalSettings"));

            services.AddMemoryCache();
            services.AddSession();

            services.AddDbContext<GiguContext>(options =>
                                 options.UseSqlServer(_config.GetConnectionString("Default")));

            services.AddIdentity<Customer, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 3;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<GiguContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IProduct, ProductRepository>();
            services.AddScoped<ICategory, CategoryRepository>();
            services.AddScoped<ISubCategory, SubCategoryRepository>();
            services.AddSingleton<IOrder, OrderRepository>();
            services.AddScoped<IOrderLine, OrderLineRepository>();
            services.AddTransient<IPicture, PictureRepository>();
            services.AddScoped<ICartItem, CartItemRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var supportedCultures = new[]
{
                    new CultureInfo("en-US"),
                    new CultureInfo("fr")
                };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<GiguContext>();
                dbContext.Database.EnsureCreated();
            }
            app.UseStaticFiles();
            /*Adler*/
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            //app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller}/{action}/{id?}"
                );
                routes.MapRoute(
                 name: "default",
                 template: "{controller=Home}/{action=Index}/{id?}"
               );
            });
        }
    }
}
