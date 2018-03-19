using System;
using System.Threading.Tasks;
using DailyScrumBag.Interfaces.Scheduling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DailyScrumBag.Scheduler.Scheduling
{
    public static class SchedulerExtensions
    {
        public static IServiceCollection AddScheduler(this IServiceCollection services)
        {
            return services.AddSingleton<IHostedService, SchedulerHostedService>();
        }

        public static IServiceCollection AddScheduler(this IServiceCollection services, EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler)
        {
            return services.AddSingleton<IHostedService, SchedulerHostedService>(serviceProvider =>
            {
                var instance = new SchedulerHostedService(serviceProvider.GetServices<IScheduledTask>());
                instance.UnobservedTaskException += unobservedTaskExceptionHandler;
                return instance;
            });
        }

        //public static IServiceCollection AddScheduler(this IServiceCollection services, EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler, IConfiguration configuration)
        //{
        //    return services.AddSingleton<IHostedService, SchedulerHostedService>(serviceProvider =>
        //    {
        //        var instance = new SchedulerHostedService(serviceProvider.GetServices<IScheduledTask>(), configuration);
        //        instance.UnobservedTaskException += unobservedTaskExceptionHandler;
        //        return instance;
        //    });
        //}
    }
}