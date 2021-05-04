using System;
using System.Globalization;
using WebApplication.Infrastructure.Enums;

namespace WebApplication.Infrastructure.Localization
{
    public static class FormattingTypes
    {
        public static string FormatCurrency(this decimal number, CultureInfoTag cultureInfoTag)
        {
            CultureInfo cultureInfo = null;
            NumberFormatInfo numberFormatInfo = null;
            switch (cultureInfoTag)
            {
                case CultureInfoTag.English_GB:
                    cultureInfo = CultureInfo.CreateSpecificCulture("en-GB");
                    break;
                case CultureInfoTag.English_US:
                    cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                    break;
                default:
                    break;
            }

            return number.ToString("C", cultureInfo);
        }

        public static string PadWithLeadingZeros(this int value, int numberOfLeadingZeros)
        {
            int decimalLength = value.ToString("D").Length + numberOfLeadingZeros;
            return value.ToString("D" + decimalLength.ToString());
        }
    }

    public static class TimeSpanExtensions
    {
        public static string ToDetailedString(this TimeSpan timeSpan, bool useTotalHours = true)
        {
            if (timeSpan == null)
                throw new ArgumentNullException("timeSpan");

            if (timeSpan.Days > 0 && !useTotalHours)
                return timeSpan.ToString("dd\\:hh\\:mm\\:ss");
            else
            {
                var totalHours = timeSpan.Days.GetDaysInHours() + timeSpan.Hours;
                return $"{totalHours}:{timeSpan.Minutes}:{timeSpan.Seconds}";
            }
        }
    }
}