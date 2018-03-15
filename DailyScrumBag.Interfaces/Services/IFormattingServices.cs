using System;
using System.Collections.Generic;
using System.Text;

namespace DailyScrumBag.Interfaces.Services
{
    public interface IFormattingServices
    {
        /// <summary>
        /// Converts Date using ToString("d")
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Readable DateTime</returns>
        string AsReadableDate(DateTime date);
    }
}
