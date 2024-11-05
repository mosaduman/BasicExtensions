using BasicExtensions.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BasicExtensions
{
    public static class StringExtensions
    {
        public static string PadLeft(this object data, char d = '0', int count = 2) => data.ToString().PadLeft(count, d);

        public static string PadRight(this object data, char d = '0', int count = 2) => data.ToString().PadRight(count, d);

        public static string ToCustomSubstring(this string data, int size = 11) => (data ?? "").Length > size ? data.Substring(0, size) : data;

        public static bool IsGuid(this string value) => Guid.TryParse(value, out _);

        public static string CreateId(
            this string prefix,
            string id,
            int length = 16,
            bool isYear = true,
            bool isMonth = false,
            bool isDay = false)
        {
            var dateComponents = new List<string> { prefix };

            if (isYear)
                dateComponents.Add(DateTime.Now.Year.ToString());
            if (isMonth)
                dateComponents.Add(DateTime.Now.Month.ToString("D2"));
            if (isDay)
                dateComponents.Add(DateTime.Now.Day.ToString("D2"));

            int remainingLength = length - (dateComponents.Sum(x => x.Length) + id.Length);
            dateComponents.Add(id.PadLeft(remainingLength, '0'));

            return string.Concat(dateComponents);
        }

        public static string ToOnlyNumericValue(this string value) => Regex.Replace(value, "[^0-9]", "");

        public static bool IsValidPassword(
            this string password,
            int minCount = 8,
            int maxCount = 32,
            bool isUpper = true,
            bool isLower = true,
            bool isDigit = false,
            bool isSpecial = false) =>
            password.Length >= minCount && password.Length <= maxCount &&
            (!isUpper || password.Any(char.IsUpper)) &&
            (!isLower || password.Any(char.IsLower)) &&
            (!isDigit || password.Any(char.IsDigit)) &&
            (!isSpecial || password.Any(char.IsPunctuation));

        public static bool IsValidEmail(this string email) =>
            !string.IsNullOrWhiteSpace(email) &&
            Regex.IsMatch(email, "^(?!\\.)(\"([^\"\\r\\\\]|\\\\[\"\\r\\\\])*\"|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\\.)\\.)*)(?<!\\.)@[a-z0-9][\\w\\.-]*[a-z0-9]\\.[a-z][a-z\\.]*[a-z]$", RegexOptions.IgnoreCase);

        public static bool IsValidTckn(this string tckn)
        {
            if (tckn.Length != 11 || !tckn.All(char.IsDigit) || tckn.StartsWith("0"))
                return false;

            int sum1 = tckn[0] + tckn[2] + tckn[4] + tckn[6] + tckn[8];
            int sum2 = tckn[1] + tckn[3] + tckn[5] + tckn[7];
            int digit10 = (sum1 * 7 - sum2) % 10;
            int digit11 = (sum1 + sum2 + tckn[9]) % 10;

            return digit10 == tckn[9] && digit11 == tckn[10];
        }

        public static bool IsValidVkn(this string vkn)
        {
            if (vkn.Length != 10 || !vkn.All(char.IsDigit))
                return false;

            int sum = Enumerable.Range(0, 9)
                .Select(i =>
                {
                    int val = (vkn[i] - '0' + (9 - i)) % 10;
                    return val == 0 ? 9 : val;
                })
                .Sum();

            int controlDigit = (10 - sum % 10) % 10;
            return controlDigit == (vkn[9] - '0');
        }

        public static bool IsValidVknOrTckn(this string kno) => kno.IsValidTckn() || kno.IsValidVkn();

        public static bool IsValidPhoneFormat(this string phone) => phone.ToPhoneFormatLength10().Length == 10;

        public static string ToPhoneFormatLength10(this string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "";

            string numericValue = phone.ToOnlyNumericValue();

            return numericValue.Length switch
            {
                10 => numericValue,
                12 when numericValue.StartsWith("90") => numericValue.Substring(2),
                11 when numericValue.StartsWith("0") => numericValue.Substring(1),
                _ => ""
            };
        }

        public static string ToPhoneFormat(this string phone, string format = "5554443322")
        {
            if (!phone.IsValidPhoneFormat())
                return "";

            string formattedPhone = phone.ToPhoneFormatLength10();

            return format switch
            {
                "+90 (555) 444 33 33" => $"+90 ({formattedPhone[..3]}) {formattedPhone[3..6]} {formattedPhone[6..8]} {formattedPhone[8..]}",
                "+90 (555) 444 3333" => $"+90 ({formattedPhone[..3]}) {formattedPhone[3..6]} {formattedPhone[6..]}",
                "0 (555) 444 33 22" => $"0 ({formattedPhone[..3]}) {formattedPhone[3..6]} {formattedPhone[6..8]} {formattedPhone[8..]}",
                "0 (555) 444 3322" => $"0 ({formattedPhone[..3]}) {formattedPhone[3..6]} {formattedPhone[6..]}",
                "05554443322" => $"0{formattedPhone}",
                _ => formattedPhone
            };
        }

        public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value);

        public static bool AnyUpperChar(this string value) => value.Any(char.IsUpper);

        public static bool AnyLowerChar(this string value) => value.Any(char.IsLower);

        public static bool AnyDigitChar(this string value) => value.Any(char.IsDigit);

        public static bool AnySpecialChar(this string value) => value.Any(char.IsPunctuation);

        public static string RemoveSpecialCharacters(this string value) => Regex.Replace(value, @"[^a-zA-Z0-9]", "");

        public static string ToSnakeCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (char.IsUpper(c) && sb.Length > 0)
                    sb.Append('_');
                sb.Append(char.ToLower(c));
            }
            return sb.ToString();
        }

        public static T GetEnumValueFromDescription<T>(this string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute?.Description == description)
                    return (T)field.GetValue(null);
            }
            throw new ArgumentException($"No enum value found for description '{description}'");
        }

        public static bool IsBase64String(this string base64)
        {
            if (string.IsNullOrWhiteSpace(base64) || base64.Length % 4 != 0 || base64.Contains(' ') || base64.Contains('\t') || base64.Contains('\r') || base64.Contains('\n'))
                return false;

            try
            {
                Convert.FromBase64String(base64);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ContainsTurkishCharacters(this string value) =>
            value.Any(c => "çÇğĞıİöÖşŞüÜ".Contains(c));

        public static string ReplaceTurkishCharacters(this string value)
        {
            Dictionary<char, char> turkishChars = new Dictionary<char, char>
            {
                {'ç', 'c'}, {'Ç', 'C'},
                {'ğ', 'g'}, {'Ğ', 'G'},
                {'ı', 'i'}, {'İ', 'I'},
                {'ö', 'o'}, {'Ö', 'O'},
                {'ş', 's'}, {'Ş', 'S'},
                {'ü', 'u'}, {'Ü', 'U'}
            };

            StringBuilder sb = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                sb.Append(turkishChars.TryGetValue(c, out char replacement) ? replacement : c);
            }
            return sb.ToString();
        }

        public static string ReverseString(this string value) => new string(value.Reverse().ToArray());
    }
}
