using System.Net.Http;

namespace DailyScrumBag.Infrastructure.Adapters
{
    /// <summary>
    /// Singleton implementation of HttpClient
    /// </summary>
    public sealed class HttpClientS
    {
        private static readonly HttpClient instance = new HttpClient();

        static HttpClientS()
        {
        }

        private HttpClientS()
        {

        }

        public static HttpClient Instance
        {
            get
            {
                return instance;
            }
        }

    }
}
