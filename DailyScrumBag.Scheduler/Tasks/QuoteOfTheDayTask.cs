using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DailyScrumBag.Interfaces.Scheduling;
using DailyScrumBag.Infrastructure.Adapters;
using DailyScrumBag.Repository.POCOs;

namespace DailyScrumBag.Scheduler.Tasks
{
    // uses https://theysaidso.com/api/
    /// <summary>
    /// Quote of the Day Crontab Scheduled Task
    /// </summary>
    public class QuoteOfTheDayTask : IScheduledTask
    {
        /// <summary>
        /// Crontab Schedule must be calculated from UTC time (EST +6)
        /// </summary>
        public string Schedule => "* */6 * * *";
        internal HttpClient _HttpClient = HttpClientS.Instance;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var quoteJson = JObject.Parse(await _HttpClient.GetStringAsync("http://quotes.rest/qod.json"));

            QuoteOfTheDay.Current = JsonConvert.DeserializeObject<QuoteOfTheDay>(quoteJson["contents"]["quotes"][0].ToString());
        }
    }
}

