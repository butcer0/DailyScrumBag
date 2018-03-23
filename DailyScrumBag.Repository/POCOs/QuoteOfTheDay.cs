using System;
using System.Collections.Generic;
using System.Text;

namespace DailyScrumBag.Repository.POCOs
{
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
