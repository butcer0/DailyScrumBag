using System;
using System.Collections.Generic;
using System.Text;

namespace DailyScrumBag.Scheduler.Helpers
{
    public static class SchedulerHelpers
    {
        public static DateTime ConvertToUTC(ref DateTime localTime)
        {
            return localTime.ToUniversalTime();
        }

        public static int ConvertToUTCMinute(int localMinute)
        {
            string strDateTime = $"12 June 2004 4:{localMinute}:01";
            DateTime localTime = DateTime.Parse(strDateTime);

            return (localTime != null) ? localTime.ToUniversalTime().Minute : DateTime.MinValue.Minute;
        }

        /// <summary>
        /// Retrieves the Hour from a Local DateTime
        /// </summary>
        /// <param name="localTime">Local DateTime</param>
        /// <returns></returns>
        public static int ConvertToUTCHour(int localHour)
        {
            string strDateTime = $"12 June 2004 {localHour}:00:01";
            DateTime localTime = DateTime.Parse(strDateTime);
            
            return (localTime != null) ? localTime.ToUniversalTime().Hour : DateTime.MinValue.Hour;
        }

    }
}
