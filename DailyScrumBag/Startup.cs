using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyScrumBag.Extensions;
using DailyScrumBag.Interfaces;
using DailyScrumBag.Interfaces.Extensions;
using DailyScrumBag.Interfaces.Services;
using DailyScrumBag.Repository.Models;
using DailyScrumBag.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DailyScrumBag
{
    public class Startup
    {
        private readonly IConfiguration _Configuration;

        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IFormattingServices, FormattingServices>();
            //TODO: Erik - 3/15/2018 Introduce all DataContexts
            #region Register DataContexts
            #endregion
            //Erik - 3/15/2018 Can set default value with GetValue(Config:Value, defaultValue);
            services.AddTransient<IFeatureToggles, FeatureToggles>(x => new FeatureToggles()
            {
                EnableDeveloperExceptions = _Configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions", false)
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<IdentityDataContext>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IFeatureToggles features)
        {
            loggerFactory.AddConsole();

            app.UseExceptionHandler("/error.html");
            #region Depricated - Use Static Error Page
            //if (env.IsDevelopment())
            //{
            //    app.UseBrowserLink();
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}
            #endregion

            if (features.EnableDeveloperExceptions)
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (AppContext, next) =>
            {
                if (AppContext.Request.Path.Value.Contains("invalid"))
                {
                    throw new Exception("Error2!");
                }
                await next();
            });

            app.UseAuthentication();
            #region Depricated - Use UseFileServer for all Static MiddleWare
            //app.UseStaticFiles();
            #endregion
            app.UseMvc(routes =>
            {
                routes.MapRoute("Default",
                    "{controller=Home}/{action=Index}/{id:int?}");
            });

            app.UseFileServer();
        }
    }
}
