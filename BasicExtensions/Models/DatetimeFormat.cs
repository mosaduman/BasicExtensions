namespace BasicExtensions.Models
{
    public static class DatetimeFormat
    {
        public static class LongDateTimeFormat
        {
            public const string TrFormat1 = "dd.MM.yyyy HH:mm:ss";
            public const string TrFormat2 = "dd/MM/yyyy HH:mm:ss";
            public const string DbFormat = "yyyy-MM-dd HH:mm:ss";
        }

        public static class ShortDateFormat
        {
            public const string TrFormat1 = "dd.MM.yyyy";
            public const string TrFormat2 = "dd/MM/yyyy";
            public const string DbFormat = "yyyy-MM-dd";
        }

        public static class TimeFormat
        {
            public const string Format1 = "HH:mm";
            public const string Format2 = "HH:mm:ss";
            public const string Format3 = "HH:mm:ss.fff";
        }
    }
}