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
        /// <summary>
        /// Truncates a String to Specific Length
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        string Chop(string s, int length);
    }
}
