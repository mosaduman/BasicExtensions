using System;
using System.Linq;

namespace BasicExtensions
{
    public static class NumberExtensitons
    {
        public static Decimal CalculatePercentage(this Decimal value, Decimal percent) => NumberExtensitons.Round(value * percent / 100M, 4);

        public static double CalculatePercentage(this double value, Decimal percent) => value.ToDecimal().CalculatePercentage(percent).ToDouble();

        public static float CalculatePercentage(this float value, Decimal percent) => value.ToDecimal().CalculatePercentage(percent).ToFloat();

        public static Decimal IncludePercentage(this Decimal price, Decimal percent) => Math.Round(price * (1.0M + percent / 100.0M), 4);

        public static double IncludePercentage(this double price, Decimal percent) => price.ToDecimal().IncludePercentage(percent).ToDouble();

        public static float IncludePercentage(this float price, Decimal percent) => price.ToDecimal().IncludePercentage(percent).ToFloat();

        public static Decimal Round(this Decimal value, int decimals = 2) => Math.Round(value, decimals);

        public static double Round(this double value, int decimals = 2) => Math.Round(value, decimals);

        public static float Round(this float value, int decimals = 2) => Math.Round(value.ToDecimal(), decimals).ToFloat();

        public static Decimal SubtractPercentage(this Decimal price, Decimal percent) => Math.Round(price / (1.0M + percent / 100.0M), 4);

        public static double SubtractPercentage(this double price, Decimal percent) => price.ToDecimal().SubtractPercentage(percent).ToDouble();

        public static float SubtractPercentage(this float price, Decimal percent) => price.ToDecimal().SubtractPercentage(percent).ToFloat();

        public static Decimal ToDecimal(this Decimal? value) => value.GetValueOrDefault();

        public static Decimal ToDecimal(this short value) => Convert.ToDecimal(value);

        public static Decimal? ToDecimal(this short? value) => !value.HasValue ? new Decimal?() : new Decimal?(Convert.ToDecimal(value.Value));

        public static Decimal ToDecimal(this int value) => Convert.ToDecimal(value);

        public static Decimal? ToDecimal(this int? value) => !value.HasValue ? new Decimal?() : new Decimal?(Convert.ToDecimal(value.Value));

        public static Decimal ToDecimal(this long value) => Convert.ToDecimal(value);

        public static Decimal? ToDecimal(this long? value) => !value.HasValue ? new Decimal?() : new Decimal?(Convert.ToDecimal(value.Value));

        public static Decimal ToDecimal(this float value) => Convert.ToDecimal(value);

        public static Decimal? ToDecimal(this float? value) => !value.HasValue ? new Decimal?() : new Decimal?(Convert.ToDecimal(value.Value));

        public static Decimal ToDecimal(this double value) => Convert.ToDecimal(value);

        public static Decimal? ToDecimal(this double? value) => !value.HasValue ? new Decimal?() : new Decimal?(Convert.ToDecimal(value.Value));

        public static Decimal ToDecimal(this string value)
        {
            Decimal result;
            return !Decimal.TryParse(value, out result) ? 0M : result;
        }

        public static Decimal? ToDecimalNullable(this string value)
        {
            Decimal result;
            return !Decimal.TryParse(value, out result) ? new Decimal?() : new Decimal?(result);
        }

        public static double ToDouble(this double? value) => value.GetValueOrDefault();

        public static double ToDouble(this short value) => Convert.ToDouble(value);

        public static double? ToDouble(this short? value) => !value.HasValue ? new double?() : new double?(Convert.ToDouble((object)value));

        public static double ToDouble(this int value) => Convert.ToDouble(value);

        public static double? ToDouble(this int? value) => !value.HasValue ? new double?() : new double?(Convert.ToDouble((object)value));

        public static double ToDouble(this long value) => Convert.ToDouble(value);

        public static double? ToDouble(this long? value) => !value.HasValue ? new double?() : new double?(Convert.ToDouble((object)value));

        public static double ToDouble(this float value) => Convert.ToDouble(value);

        public static double? ToDouble(this float? value) => !value.HasValue ? new double?() : new double?(Convert.ToDouble((object)value));

        public static double ToDouble(this Decimal value) => Convert.ToDouble(value);

        public static double? ToDouble(this Decimal? value) => !value.HasValue ? new double?() : new double?(Convert.ToDouble((object)value));

        public static double ToDouble(this string value)
        {
            double result;
            return !double.TryParse(value, out result) ? 0.0 : result;
        }

        public static double? ToDoubleNullable(this string value)
        {
            double result;
            return !double.TryParse(value, out result) ? new double?() : new double?(result);
        }

        public static float ToFloat(this float? value) => value.GetValueOrDefault();

        public static float ToFloat(this short value) => Convert.ToSingle(value);

        public static float? ToFloat(this short? value) => !value.HasValue ? new float?() : new float?(Convert.ToSingle((object)value));

        public static float ToFloat(this int value) => Convert.ToSingle(value);

        public static float? ToFloat(this int? value) => !value.HasValue ? new float?() : new float?(Convert.ToSingle((object)value));

        public static float ToFloat(this long value) => Convert.ToSingle(value);

        public static float? ToFloat(this long? value) => !value.HasValue ? new float?() : new float?(Convert.ToSingle((object)value));

        public static float ToFloat(this double value) => Convert.ToSingle(value);

        public static float? ToFloat(this double? value) => !value.HasValue ? new float?() : new float?(Convert.ToSingle((object)value));

        public static float ToFloat(this Decimal value) => Convert.ToSingle(value);

        public static float? ToFloat(this Decimal? value) => !value.HasValue ? new float?() : new float?(Convert.ToSingle((object)value));

        public static float ToFloat(this string value)
        {
            float result;
            return !float.TryParse(value, out result) ? 0.0f : result;
        }

        public static float? ToFloatNullable(this string value)
        {
            float result;
            return !float.TryParse(value, out result) ? new float?() : new float?(result);
        }

        public static int ToInt(this int? value) => value.GetValueOrDefault();

        public static int ToInt(this short value) => (int)value;

        public static int? ToInt(this short? value)
        {
            short? nullable = value;
            return !nullable.HasValue ? new int?() : new int?((int)nullable.GetValueOrDefault());
        }

        public static int ToInt(this long value) => (int)value;

        public static int? ToInt(this long? value)
        {
            long? nullable = value;
            return !nullable.HasValue ? new int?() : new int?((int)nullable.GetValueOrDefault());
        }

        public static int ToInt(this float value) => (int)value;

        public static int? ToInt(this float? value)
        {
            float? nullable = value;
            return !nullable.HasValue ? new int?() : new int?((int)nullable.GetValueOrDefault());
        }

        public static int ToInt(this double value) => (int)value;

        public static int? ToInt(this double? value)
        {
            double? nullable = value;
            return !nullable.HasValue ? new int?() : new int?((int)nullable.GetValueOrDefault());
        }

        public static int ToInt(this Decimal value) => (int)value;

        public static int? ToInt(this Decimal? value)
        {
            Decimal? nullable = value;
            return !nullable.HasValue ? new int?() : new int?((int)nullable.GetValueOrDefault());
        }

        public static int ToInt(this string value)
        {
            int result;
            return !int.TryParse(value, out result) ? 0 : result;
        }

        public static int? ToIntNullable(this string value)
        {
            int result;
            return !int.TryParse(value, out result) ? new int?() : new int?(result);
        }

        public static long ToLong(this long? value) => value.GetValueOrDefault();

        public static long ToLong(this short value) => (long)value;

        public static long? ToLong(this short? value)
        {
            short? nullable = value;
            return !nullable.HasValue ? new long?() : new long?((long)nullable.GetValueOrDefault());
        }

        public static long ToLong(this int value) => (long)value;

        public static long? ToLong(this int? value)
        {
            int? nullable = value;
            return !nullable.HasValue ? new long?() : new long?((long)nullable.GetValueOrDefault());
        }

        public static long ToLong(this float value) => (long)value;

        public static long? ToLong(this float? value)
        {
            float? nullable = value;
            return !nullable.HasValue ? new long?() : new long?((long)nullable.GetValueOrDefault());
        }

        public static long ToLong(this double value) => (long)value;

        public static long? ToLong(this double? value)
        {
            double? nullable = value;
            return !nullable.HasValue ? new long?() : new long?((long)nullable.GetValueOrDefault());
        }

        public static long ToLong(this Decimal value) => (long)value;

        public static long? ToLong(this Decimal? value)
        {
            Decimal? nullable = value;
            return !nullable.HasValue ? new long?() : new long?((long)nullable.GetValueOrDefault());
        }

        public static long ToLong(this string value)
        {
            long result;
            return !long.TryParse(value, out result) ? 0L : result;
        }

        public static long? ToLongNullable(this string value)
        {
            long result;
            return !long.TryParse(value, out result) ? new long?() : new long?(result);
        }

        public static short ToShort(this short? value) => value.GetValueOrDefault();

        public static short ToShort(this long value) => (short)value;

        public static short? ToShort(this long? value)
        {
            long? nullable = value;
            return !nullable.HasValue ? new short?() : new short?((short)nullable.GetValueOrDefault());
        }

        public static short ToShort(this int value) => (short)value;

        public static short? ToShort(this int? value)
        {
            int? nullable = value;
            return !nullable.HasValue ? new short?() : new short?((short)nullable.GetValueOrDefault());
        }

        public static short ToShort(this float value) => (short)value;

        public static short? ToShort(this float? value)
        {
            float? nullable = value;
            return !nullable.HasValue ? new short?() : new short?((short)nullable.GetValueOrDefault());
        }

        public static short ToShort(this double value) => (short)value;

        public static short? ToShort(this double? value)
        {
            double? nullable = value;
            return !nullable.HasValue ? new short?() : new short?((short)nullable.GetValueOrDefault());
        }

        public static short ToShort(this Decimal value) => (short)value;

        public static short? ToShort(this Decimal? value)
        {
            Decimal? nullable = value;
            return !nullable.HasValue ? new short?() : new short?((short)nullable.GetValueOrDefault());
        }

        public static short ToShort(this string value)
        {
            short result;
            return !short.TryParse(value, out result) ? (short)0 : result;
        }

        public static short? ToShortNullable(this string value)
        {
            short result;
            return !short.TryParse(value, out result) ? new short?() : new short?(result);
        }

        public static Decimal ToRealDecimal(this string value)
        {
            value = value.Trim();
            value = value.StartsWith(",") || value.StartsWith(".") ? "0" + value : value;
            if (string.IsNullOrWhiteSpace(value) || value.Replace(".", "").Replace(",", "").Any<char>((Func<char, bool>)(s => !char.IsDigit(s))))
                return 0M;
            if (!value.Any<char>((Func<char, bool>)(s => s == '.' || s == ',')))
                return value.ToDecimal();
            int length = value.IndexOf('.') != -1 ? value.IndexOf('.') : value.IndexOf(',');
            int num1 = value.Substring(0, length).ToInt();
            int num2 = num1 + 1;
            Decimal realDecimal = value.Replace(',', '.').ToDecimal();
            Decimal num3 = value.Replace('.', ',').ToDecimal();
            if (realDecimal >= (Decimal)num1 && realDecimal <= (Decimal)num2)
                return realDecimal;
            return !(num3 >= (Decimal)num1) || !(num3 <= (Decimal)num2) ? (Decimal)num1 : num3;
        }

        public static int ToProcessing(this int total, int value)
        {
            Decimal percentage = total.ToDecimal().CalculatePercentage(value.ToDecimal());
            return (percentage < 0M ? 0M : (percentage > 100M ? 100M : percentage)).ToInt();
        }
    }
}
