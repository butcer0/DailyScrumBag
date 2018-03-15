using DailyScrumBag.Interfaces.Services;
using System;

namespace DailyScrumBag.Services
{
    public class FormattingServices : IFormattingServices
    {
        /// <summary>
        /// Converts Date using ToString("d")
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Readable DateTime</returns>
        public string AsReadableDate(DateTime date)
        {
            return date.ToString("d");
        }
    }
}
