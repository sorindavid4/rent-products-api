using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rent_products_api.ServiceLayer.Utils
{
    public static class GenericFunctions
    {
        public static DateTime GetCurrentDateTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, GetEuropeTimeZone());
        }

        public static TimeSpan ParseStringToTimeSpan(string timeAsString)
        {
            if (string.IsNullOrEmpty(timeAsString))
            {
                return TimeSpan.Zero;
            }
            var timeAsTimeSpan = TimeSpan.Parse(timeAsString);
            return timeAsTimeSpan;
        }
        public static string ParseTimeSpanToString(TimeSpan time)
        {
            if (time == TimeSpan.Zero)
            {
                return "00:00";
            }
            var timeAsString = time.ToString(@"hh\:mm");
            return timeAsString;
        }

        public static string ParseDateTime(DateTime dateTime)
        {
            {
                var date = new DateTime().ToString("yyyy-MM-dd HH:mm");
                if ((dateTime.ToString("yyyy-MM-dd HH:mm").Equals(date)))
                {
                    return null;
                }
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, GetEuropeTimeZone()).ToString("yyyy-MM-dd HH:mmZ");
            }

        }
        public static string ParseNullableDateTime(DateTime? dateTime)
        {
            if (dateTime == null || (bool)dateTime?.ToString("yyyy-MM-dd HH:mm").Equals(new DateTime().ToString("yyyy-MM-dd HH:mm")))
            {
                return null;
            }
            else
            {
                DateTime data = TimeZoneInfo.ConvertTimeToUtc((DateTime)dateTime, GetEuropeTimeZone());
                return data.ToString("yyyy-MM-dd HH:mmZ");
            }
        }

        public static string ParseNullableDateTimeAsDisplayableString(DateTime? dateTime)
        {
            if (dateTime == null || (bool)dateTime?.ToString("yyyy-MM-dd HH:mm").Equals(new DateTime().ToString("yyyy-MM-dd HH:mm")))
            {
                return null;
            }
            else
            {
                DateTime data = (DateTime)dateTime;
                return data.ToString("yyyy-MM-dd");
            }
        }

        public static DateTime? ParseStringToDateTime(string dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }
            var dateAsDate = Convert.ToDateTime(dateTime).ToUniversalTime();
            DateTime date = TimeZoneInfo.ConvertTimeFromUtc(dateAsDate, GetEuropeTimeZone());
            return date;
        }


        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static string GetFileNameHashed(string fileName)
        {
            return Guid.NewGuid().ToString("N") + Path.GetExtension(fileName);
        }

        public static string SeparateStringByCapitals(string stringForEnum)
        {
            string theString = stringForEnum;
            StringBuilder builder = new StringBuilder();
            foreach (char c in theString)
            {
                if (Char.IsUpper(c) && builder.Length > 0) builder.Append(' ');
                builder.Append(c);
            }
            theString = builder.ToString();
            return theString;
        }

        public static string SerializeObject(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static DateTime ConvertDateAndTimeUtcToLocal(string utcDateAsString, string utcTimeAsString)
        {
            var utcDate = Convert.ToDateTime(utcDateAsString).ToUniversalTime();
            var date = TimeZoneInfo.ConvertTimeFromUtc(utcDate, GetEuropeTimeZone());
            var utcTime = Convert.ToDateTime(utcTimeAsString).ToUniversalTime();
            var time = TimeZoneInfo.ConvertTimeFromUtc(utcTime, GetEuropeTimeZone()).ToString("HH:mm");
            var timeSpan = TimeSpan.Parse(time);
            return new DateTime(date.Year, date.Month, date.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        }

        public static TimeZoneInfo GetEuropeTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time");
            }
            catch (Exception ex)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest");
            }
        }
    }
}
