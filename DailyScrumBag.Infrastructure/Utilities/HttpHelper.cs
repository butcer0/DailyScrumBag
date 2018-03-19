using Microsoft.AspNetCore.Http;

namespace DailyScrumBag.Infrastructure.Utilities
{
    /// <summary>
    /// contains http helpers.
    /// </summary>
    public static class HttpHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// sets httpContextAccessor.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext HttpContext
        {
            get
            {
                return _httpContextAccessor.HttpContext;
            }
        }
    }
}