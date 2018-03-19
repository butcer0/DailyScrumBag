using DailyScrumBag.Interfaces.Services;
using System;
using System.Text;

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

        /// <summary>
        /// Truncates a String to Specific Length
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string Chop(string s,int length)
        {
            if (String.IsNullOrEmpty(s))
                throw new ArgumentNullException(s);
            var words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words[0].Length > length)
                return words[0];
            var sb = new StringBuilder();

            foreach (var word in words)
            {
                if ((sb + word).Length > length)
                    return string.Format("{0}", sb.ToString().TrimEnd(' '));
                sb.Append(word + " ");
            }
            return string.Format("{0}", sb.ToString().TrimEnd(' '));
        }
    }
}
