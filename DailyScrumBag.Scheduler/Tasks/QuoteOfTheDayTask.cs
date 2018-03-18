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

namespace DailyScrumBag.Scheduler.Tasks
{
    // uses https://theysaidso.com/api/
    public class QuoteOfTheDayTask : IScheduledTask
    {
        public string Schedule => "* */6 * * *";
        internal HttpClient _HttpClient = HttpClientS.Instance;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            #region Depricated - Use Singleton HttpClient
            //var httpClient = new HttpClient();
            #endregion

            var quoteJson = JObject.Parse(await _HttpClient.GetStringAsync("http://quotes.rest/qod.json"));

            QuoteOfTheDay.Current = JsonConvert.DeserializeObject<QuoteOfTheDay>(quoteJson["contents"]["quotes"][0].ToString());
        }
    }

    public class QuoteOfTheDay
    {
        public static QuoteOfTheDay Current { get; set; }

        static QuoteOfTheDay()
        {
            Current = new QuoteOfTheDay { Quote = "No quote", Author = "Erik B" };
        }

        public string Quote { get; set; }
        public string Author { get; set; }
    }
}

