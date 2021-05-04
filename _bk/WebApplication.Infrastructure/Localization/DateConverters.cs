using System;

namespace WebApplication.Infrastructure.Localization
{
    public static class DateConverters
    {
        private const int HOURS_IN_A_DAY = 24;
        private const int MINUTES_IN_A_HOUR = 60;
        private const int SECONDS_INT_A_MINUTE = 60;
        public const long TICKS_PER_SECOND = 10000000;

        #region Days Converters
        public static int GetDaysInHours(this int numberOfDays)
        {
            return numberOfDays * HOURS_IN_A_DAY;
        }

        public static int GetDaysInMinutes(this int numberOfDays)
        {
            return numberOfDays * HOURS_IN_A_DAY * MINUTES_IN_A_HOUR;
        }

        public static int GetDaysInSeconds(this int numberOfDays)
        {
            return numberOfDays * HOURS_IN_A_DAY * MINUTES_IN_A_HOUR * SECONDS_INT_A_MINUTE;
        }

        public static int GetDaysFromHours(this int hours)
        {
            return hours / HOURS_IN_A_DAY;
        }

        public static int GetDaysFromMinutes(this int minutes)
        {
            int hours = minutes / MINUTES_IN_A_HOUR;
            return hours / HOURS_IN_A_DAY;
        }

        public static int GetDaysFromSeconds(this int seconds)
        {
            int minutes = seconds / SECONDS_INT_A_MINUTE;
            int hours = minutes / MINUTES_IN_A_HOUR;
            return hours / HOURS_IN_A_DAY;
        }
        #endregion

        #region Hours Converters

        public static int GetHoursInMinutes(this int numberOfHours)
        {
            return numberOfHours * MINUTES_IN_A_HOUR;
        }

        public static int GetHoursInSeconds(this int numberOfHours)
        {
            return numberOfHours * MINUTES_IN_A_HOUR * SECONDS_INT_A_MINUTE;
        }

        public static int GetHoursFromMinutes(this int minutes)
        {
            return minutes / MINUTES_IN_A_HOUR;
        }

        public static int GetHoursFromSeconds(this int seconds)
        {
            int minutes = seconds / SECONDS_INT_A_MINUTE;
            return minutes / MINUTES_IN_A_HOUR;
        }
        #endregion

        #region Minutes Converters
        public static int GetMinutesInSeconds(this int numberOfMinutes)
        {
            return numberOfMinutes * SECONDS_INT_A_MINUTE;
        }

        public static int GetMinutesFromSeconds(this int seconds)
        {
            return seconds / SECONDS_INT_A_MINUTE;
        }
        #endregion

        #region Seconds Converters
        public static long GetTicksFromSeconds(this int seconds)
        {
            return seconds * TICKS_PER_SECOND;
        }
        #endregion

        //return days,hours,mins and seconds from total seconds
        public static Duration GetDurationFromSeconds(this int inputInSeconds)
        {
            int day = inputInSeconds / (24 * 3600);

            inputInSeconds = inputInSeconds % (24 * 3600);
            int hour = inputInSeconds / 3600;

            inputInSeconds %= 3600;
            int minutes = inputInSeconds / 60;

            inputInSeconds %= 60;
            int seconds = inputInSeconds;

            return new Duration
            {
                Days = day,
                Hours = hour,
                Minutes = minutes,
                Seconds = seconds
            };
        }

        public static int GetSecondsFromDuration(this Duration duration)
        {
            if (duration != null)
            {
                return GetDaysInSeconds(duration.Days) + GetHoursInSeconds(duration.Hours)
                    + GetMinutesInSeconds(duration.Minutes) + duration.Seconds;
            }
            return 0;
        }

        public static string GetDurationFormatted(this Duration duration)
        {
            if (duration != null)
            {
                string format = "";
                if (duration.Days > 0)
                {
                    format += duration.Days + " " + "day(s)";
                }
                if (duration.Hours > 0)
                {
                    format += format != "" ? " , " : "";
                    format += duration.Hours + " " + "hour(s)";
                }
                if (duration.Minutes > 0)
                {
                    format += format != "" ? " , " : "";
                    format += duration.Minutes + " " + "minute(s)";
                }
                if (duration.Seconds > 0)
                {
                    format += format != "" ? " , " : "";
                    format += duration.Seconds + " " + "second(s)";
                }
                return format != "" ? format : "-";
            }
            return "-";
        }
    }

    public class Duration
    {
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }

    public class TimePeriod
    {
        public TimePeriod(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
