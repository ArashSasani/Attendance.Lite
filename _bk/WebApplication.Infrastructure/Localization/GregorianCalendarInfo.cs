using System;
using System.Globalization;
using WebApplication.Infrastructure.Enums;

namespace WebApplication.Infrastructure.Localization
{
    public static class GregorianCalendarInfo
    {
        private static readonly GregorianCalendar GC = new GregorianCalendar();

        public static DateTime MinSupportedDateTime
        {
            get
            {
                return GC.MinSupportedDateTime;
            }
        }

        public static DateTime MaxSupportedDateTime
        {
            get
            {
                return GC.MaxSupportedDateTime;
            }
        }

        public static bool IsLeapYearGC(this int year)
        {
            return GC.IsLeapYear(year);
        }

        public static int GetDayOfYearGC(this DateTime date)
        {
            return GC.GetDayOfYear(date);
        }

        public static int GetDayOfMonthGC(this DateTime date)
        {
            return GC.GetDayOfMonth(date);
        }

        public static string GetMonthInNameGC(this DateTime date, CultureInfoTag culture
            , OutputDateFormat format)
        {
            int month = GC.GetMonth(date);

            return GetMonthInNameGC(month, culture, format);
        }

        public static string GetMonthInNameGC(this int month, CultureInfoTag culture
            , OutputDateFormat format)
        {
            string result = "";

            CultureInfo cultureInfo = null;
            switch (culture)
            {
                case CultureInfoTag.English_GB:
                    cultureInfo = CultureInfo.CreateSpecificCulture("en-GB");
                    break;
                case CultureInfoTag.English_US:
                    cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                    break;
            }
            var info = cultureInfo.DateTimeFormat;
            switch (format)
            {
                case OutputDateFormat.Complete:
                    result = info.GetMonthName(month);
                    break;
                case OutputDateFormat.ShortForm:
                    result = info.GetAbbreviatedMonthName(month);
                    break;
                default:
                    break;
            }
            return result;
        }

        public static string GetDayOfWeekGC(this DateTime date, CultureInfoTag culture
            , OutputDateFormat format)
        {
            var dayOfWeek = GC.GetDayOfWeek(date);

            return GetDayOfWeekGC(dayOfWeek, culture, format);
        }

        public static string GetDayOfWeekGC(this DayOfWeek dayOfWeek, CultureInfoTag culture
            , OutputDateFormat format)
        {
            string result = "";

            CultureInfo cultureInfo = null;
            switch (culture)
            {
                case CultureInfoTag.English_GB:
                    cultureInfo = CultureInfo.CreateSpecificCulture("en-GB");
                    break;
                case CultureInfoTag.English_US:
                    cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                    break;
            }
            var info = cultureInfo.DateTimeFormat;
            switch (format)
            {
                case OutputDateFormat.Complete:
                    result = info.GetDayName(dayOfWeek);
                    break;
                case OutputDateFormat.ShortForm:
                    result = info.GetAbbreviatedDayName(dayOfWeek);
                    break;
            }
            return result;
        }

        public static string GetElapsedTimeGC(this DateTime date)
        {
            string result = "";

            TimeSpan elapsed = DateTime.Now.Subtract(date);
            double? daysAgo, hoursAgo, minutesAgo, secondsAgo;
            daysAgo = hoursAgo = minutesAgo = secondsAgo = null;

            if (elapsed.Days > 0)
                daysAgo = elapsed.TotalDays;
            else if (elapsed.Hours > 0)
                hoursAgo = elapsed.TotalHours;
            else if (elapsed.Minutes > 0)
                minutesAgo = elapsed.TotalMinutes;
            else if (elapsed.Seconds > 0)
                secondsAgo = elapsed.TotalSeconds;
            else
            {
                throw new InvalidOperationException("cannot get elapsed time," +
                        $" the date: '{date}' that has been " +
                        "passed here is in the future.");
            }

            if (daysAgo.HasValue)
            {
                if ((int)daysAgo == 1)
                    result = "yesterday";
                else
                    result = (int)daysAgo.Value + " days ago ";
            }
            else if (hoursAgo.HasValue)
                result = (int)hoursAgo.Value + " hours(s) age ";
            else if (minutesAgo.HasValue)
                result = (int)minutesAgo.Value + " minute(s) ago ";
            else if (secondsAgo.HasValue)
            {
                if ((int)secondsAgo < 30)
                    result = " recently ";
                else
                    result = (int)secondsAgo.Value + " seconds ago ";
            }

            return result;
        }

    }
}
