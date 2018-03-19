using System;
using DailyScrumBag.Extensions;
using DailyScrumBag.Interfaces.Extensions;
using DailyScrumBag.Interfaces.Scheduling;
using DailyScrumBag.Interfaces.Services;
using DailyScrumBag.Repository.Helpers;
using DailyScrumBag.Repository.Repositories;
using DailyScrumBag.Scheduler.Scheduling;
using DailyScrumBag.Scheduler.Tasks;
using DailyScrumBag.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DailyScrumBag
{
    public class Startup
    {
        private readonly IConfiguration _Configuration;
        private DSDBContext _DSDBContext;

        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // Updated in Development
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IFormattingServices, FormattingServices>();

            //Erik - 3/19/2018 Move Cron Tasks to Web Application
            //var dbConnectionString = _Configuration.GetValue<string>("WebConfiguration:IdentitySettings:ConnectionString");
            //DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder<DSDBContext>();
            //dbContextOptionsBuilder.UseSqlServer(dbConnectionString);
            //_DSDBContext =new DSDBContext(dbContextOptionsBuilder.Options);

            services.AddDbContext<DSDBContext>(options =>
            {
                var connectionString = _Configuration.GetValue<string>("WebConfiguration:DatabaseSetting:ConnectionString");
                options.UseSqlServer(connectionString);

            });

          
            services.AddDbContext<IdentityDataContext>(options =>
            {
                var connectionString = _Configuration.GetValue<string>("WebConfiguration:IdentitySettings:ConnectionString");
                options.UseSqlServer(connectionString);
            });

            //Erik - 3/15/2018 Can set default value with GetValue(Config:Value, defaultValue);
            services.AddTransient<IFeatureToggles, FeatureToggles>(x => new FeatureToggles()
            {
                EnableDeveloperExceptions = _Configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions", false)
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<IdentityDataContext>();
            // Add scheduled tasks & scheduler
            services.AddSingleton<IScheduledTask, QuoteOfTheDayTask>();
            services.AddSingleton<IScheduledTask, EmailDailyTask>();
            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });

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
