using System;
using System.Collections.Generic;
using System.Globalization;

namespace BasicExtensions
{
    public static class DateExtensions
    {
        public static IEnumerable<DateTime> EachDay(this DateTime from, DateTime to)
        {
            for (DateTime day = from.Date; day.Date <= to.Date; day = day.AddDays(1.0))
                yield return day;
        }

        public static string ToDatetimeString(this DateTime date, string format = "yyyy-MM-dd")
        {
            string empty = string.Empty;
            string datetimeString;
            switch (format)
            {
                case "HH:mm":
                    datetimeString = date.Hour.PadLeft() + ":" + date.Minute.PadLeft();
                    break;
                case "HH:mm:ss":
                    datetimeString = date.Hour.PadLeft() + ":" + date.Minute.PadLeft() + ":" + date.Second.PadLeft();
                    break;
                case "HH:mm:ss.fff":
                    datetimeString = date.Hour.PadLeft() + ":" + date.Minute.PadLeft() + ":" + date.Second.PadLeft() + "." + StringExtensions.PadRight(date.Millisecond.ToString().ToCustomSubstring(3), count: 3);
                    break;
                case "dd.MM.yyyy":
                    datetimeString = string.Format("{0}.{1}.{2}", (object)date.Day.PadLeft(), (object)date.Month.PadLeft(), (object)date.Year);
                    break;
                case "dd.MM.yyyy HH:mm:ss":
                    datetimeString = string.Format("{0}.{1}.{2} {3}:{4}:{5}", (object)date.Day.PadLeft(), (object)date.Month.PadLeft(), (object)date.Year, (object)date.Hour.PadLeft(), (object)date.Minute.PadLeft(), (object)date.Second.PadLeft());
                    break;
                case "dd/MM/yyyy":
                    datetimeString = string.Format("{0}/{1}/{2}", (object)date.Day.PadLeft(), (object)date.Month.PadLeft(), (object)date.Year);
                    break;
                case "dd/MM/yyyy HH:mm:ss":
                    datetimeString = string.Format("{0}/{1}/{2} {3}:{4}:{5}", (object)date.Day.PadLeft(), (object)date.Month.PadLeft(), (object)date.Year, (object)date.Hour.PadLeft(), (object)date.Minute.PadLeft(), (object)date.Second.PadLeft());
                    break;
                case "yyyy-MM-dd":
                    datetimeString = string.Format("{0}-{1}-{2}", (object)date.Year, (object)date.Month.PadLeft(), (object)date.Day.PadLeft());
                    break;
                case "yyyy-MM-dd HH:mm:ss":
                    datetimeString = string.Format("{0}-{1}-{2} {3}:{4}:{5}", (object)date.Year, (object)date.Month.PadLeft(), (object)date.Day.PadLeft(), (object)date.Hour.PadLeft(), (object)date.Minute.PadLeft(), (object)date.Second.PadLeft());
                    break;
                default:
                    datetimeString = string.Format("{0}-{1}-{2}", (object)date.Year, (object)date.Month.PadLeft(), (object)date.Day.PadLeft());
                    break;
            }
            return datetimeString;
        }

        public static DateTime ToStartDatetime(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

        public static DateTime ToEndDatetime(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

        public static string ToStartDatetimeString(this DateTime date, string format = "yyyy-MM-dd") => date.ToStartDatetime().ToDatetimeString(format);

        public static string ToEndDatetimeString(this DateTime date, string format = "yyyy-MM-dd") => date.ToEndDatetime().ToDatetimeString(format);

        public static DateTime ToDatetime(this string date)
        {
            DateTime result;
            return !DateTime.TryParse(date, out result) ? DateTime.MinValue : result;
        }

        public static DateTime ToDatetime(this string date, IFormatProvider provider)
        {
            DateTime result;
            return !DateTime.TryParse(date, provider, DateTimeStyles.None, out result) ? DateTime.MinValue : result;
        }

        public static DateTime ToDatetime(
          this string date,
          IFormatProvider provider,
          DateTimeStyles styles)
        {
            DateTime result;
            return !DateTime.TryParse(date, provider, styles, out result) ? DateTime.MinValue : result;
        }

        public static DateTime? ToDatetimeNullable(this string date)
        {
            DateTime result;
            return !DateTime.TryParse(date, out result) ? new DateTime?() : new DateTime?(result);
        }

        public static DateTime? ToDatetimeNullable(this string date, IFormatProvider provider)
        {
            DateTime result;
            return !DateTime.TryParse(date, provider, DateTimeStyles.None, out result) ? new DateTime?() : new DateTime?(result);
        }

        public static DateTime? ToDatetimeNullable(
          this string date,
          IFormatProvider provider,
          DateTimeStyles styles)
        {
            DateTime result;
            return !DateTime.TryParse(date, provider, styles, out result) ? new DateTime?() : new DateTime?(result);
        }

        public static DateTime ToFirstDateOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

        public static DateTime ToLastDateOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

        public static bool IsWeekend(this DateTime value) => value.DayOfWeek == DayOfWeek.Saturday || value.DayOfWeek == DayOfWeek.Sunday;
    }
}
