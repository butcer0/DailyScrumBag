using System.Threading;
using System.Threading.Tasks;

namespace DailyScrumBag.Interfaces.Scheduling
{
    public interface IScheduledTask
    {
        /// <summary>
        /// Crontab Style Schedule String
        /// </summary>
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}