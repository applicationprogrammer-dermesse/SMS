using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS
{
    public class DateClass
    {
        public static string getSday(string theFirstDay)
        {
            DateTime now = DateTime.Now;
            string startDate = new DateTime(now.Year, now.Month, 1).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            return startDate;
        }

        public static string getLday(string theLastDay)
        {
            DateTime now = DateTime.Now;

            string EndDate = DateTime.Now.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            return EndDate;
        }

    }
}