using DailyScrumBag.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyScrumBag.Repository.Extensions
{
    /// <summary>
    /// InitializationExtension, handles logic related to Initialization.
    /// </summary>
    public static class InitializationExtension
    {
        public static void AddRepositoryServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DSDBContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("WebConfiguration:Setting:ServiceUrl");
                options.UseSqlServer(connectionString);

            });
        }

    }
}
