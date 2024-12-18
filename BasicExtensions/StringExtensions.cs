using BasicExtensions.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static bool IsGuid(this string value) => Guid.TryParse(value, out Guid _);

        public static string CreateId(
          this string prefix,
          string id,
          int length = 16,
          bool isYear = true,
          bool isMonth = false,
          bool isDay = false)
        {
            string[] strArray = new string[5]
            {
        prefix,
        null,
        null,
        null,
        null
            };
            int num;
            string str1;
            if (!isYear)
            {
                str1 = "";
            }
            else
            {
                num = DateTime.Now.Year;
                str1 = num.ToString();
            }
            strArray[1] = str1;
            string str2;
            if (!isMonth)
            {
                str2 = "";
            }
            else
            {
                num = DateTime.Now.Month;
                str2 = num.ToString().PadLeft();
            }
            strArray[2] = str2;
            string str3;
            if (!isDay)
            {
                str3 = "";
            }
            else
            {
                num = DateTime.Now.Day;
                str3 = num.ToString().PadLeft();
            }
            strArray[3] = str3;
            strArray[4] = StringExtensions.PadLeft(id, count: (length - (prefix.Length + id.Length + (isYear ? 4 : 0) + (isMonth ? 2 : 0) + (isDay ? 2 : 0)) + id.Length));
            return string.Concat(strArray);
        }

        public static string ToOnlyNumericValue(this string value) => Regex.Replace(value, "[^0-9]", "");

        public static bool IsValidPassword(
          this string password,
          int minCount = 8,
          int maxCount = 32,
          bool isUpper = true,
          bool isLower = true,
          bool isDigit = false,
          bool isSpecial = false)
        {
            return password.Length >= minCount && password.Length <= maxCount && (!isUpper || password.AnyUpperChar()) && (!isLower || password.AnyLowerChar()) && (!isDigit || password.AnyDigitChar()) && (!isSpecial || password.AnySpecialChar());
        }

        public static bool AnyUpperChar(this string value) => ((IEnumerable<char>)value.ToArray<char>()).Any<char>((Func<char, bool>)(s => char.IsUpper(s)));

        public static bool AnyLowerChar(this string value) => ((IEnumerable<char>)value.ToArray<char>()).Any<char>((Func<char, bool>)(s => char.IsLower(s)));

        public static bool AnyDigitChar(this string value) => ((IEnumerable<char>)value.ToArray<char>()).Any<char>((Func<char, bool>)(s => char.IsDigit(s)));

        public static bool AnySpecialChar(this string value) => ((IEnumerable<char>)value.ToArray<char>()).Any<char>((Func<char, bool>)(s => char.IsPunctuation(s)));

        public static bool IsValidEmail(this string email) => !string.IsNullOrWhiteSpace(email) && new Regex("^(?!\\.)(\"([^\"\\r\\\\]|\\\\[\"\\r\\\\])*\"|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\\.)\\.)*)(?<!\\.)@[a-z0-9][\\w\\.-]*[a-z0-9]\\.[a-z][a-z\\.]*[a-z]$", RegexOptions.IgnoreCase).IsMatch(email);

        public static bool IsValidTckn(this string tckn)
        {
            if (string.IsNullOrWhiteSpace(tckn) || !tckn.All<char>(new Func<char, bool>(char.IsDigit)) || tckn.Length != 11 || tckn.StartsWith("0"))
                return false;
            int num1 = Convert.ToInt32(tckn[0].ToString()) + Convert.ToInt32(tckn[2].ToString()) + Convert.ToInt32(tckn[4].ToString()) + Convert.ToInt32(tckn[6].ToString()) + Convert.ToInt32(tckn[8].ToString());
            int num2 = Convert.ToInt32(tckn[1].ToString()) + Convert.ToInt32(tckn[3].ToString()) + Convert.ToInt32(tckn[5].ToString()) + Convert.ToInt32(tckn[7].ToString());
            int num3 = (num1 * 7 - num2) % 10;
            int num4 = (num1 + num2 + Convert.ToInt32(tckn[9].ToString())) % 10;
            int num5 = num3;
            char ch = tckn[9];
            int int32_1 = Convert.ToInt32(ch.ToString());
            if (num5 != int32_1)
                return false;
            int num6 = num4;
            ch = tckn[10];
            int int32_2 = Convert.ToInt32(ch.ToString());
            return num6 == int32_2;
        }

        public static bool IsValidVkn(this string vkn)
        {
            if (string.IsNullOrWhiteSpace(vkn) || !vkn.All<char>(new Func<char, bool>(char.IsDigit)) || vkn.Length != 10)
                return false;
            int num1 = (Convert.ToInt32(vkn[0].ToString()) + 9) % 10;
            int num2 = (Convert.ToInt32(vkn[1].ToString()) + 8) % 10;
            int num3 = (Convert.ToInt32(vkn[2].ToString()) + 7) % 10;
            int num4 = (Convert.ToInt32(vkn[3].ToString()) + 6) % 10;
            int num5 = (Convert.ToInt32(vkn[4].ToString()) + 5) % 10;
            int num6 = (Convert.ToInt32(vkn[5].ToString()) + 4) % 10;
            int num7 = (Convert.ToInt32(vkn[6].ToString()) + 3) % 10;
            int num8 = (Convert.ToInt32(vkn[7].ToString()) + 2) % 10;
            int num9 = (Convert.ToInt32(vkn[8].ToString()) + 1) % 10;
            int int32 = Convert.ToInt32(vkn[9].ToString());
            int num10 = num1 * 512 % 9;
            int num11 = num2 * 256 % 9;
            int num12 = num3 * 128 % 9;
            int num13 = num4 * 64 % 9;
            int num14 = num5 * 32 % 9;
            int num15 = num6 * 16 % 9;
            int num16 = num7 * 8 % 9;
            int num17 = num8 * 4 % 9;
            int num18 = num9 * 2 % 9;
            if (num1 != 0 && num10 == 0)
                num10 = 9;
            if (num2 != 0 && num11 == 0)
                num11 = 9;
            if (num3 != 0 && num12 == 0)
                num12 = 9;
            if (num4 != 0 && num13 == 0)
                num13 = 9;
            if (num5 != 0 && num14 == 0)
                num14 = 9;
            if (num6 != 0 && num15 == 0)
                num15 = 9;
            if (num7 != 0 && num16 == 0)
                num16 = 9;
            if (num8 != 0 && num17 == 0)
                num17 = 9;
            if (num9 != 0 && num18 == 0)
                num18 = 9;
            int num19 = num10 + num11 + num12 + num13 + num14 + num15 + num16 + num17 + num18;
            return (num19 % 10 != 0 ? 10 - num19 % 10 : 0) == int32;
        }

        public static bool IsValidVknOrTckn(this string kno) => kno.IsValidTckn() || kno.IsValidVkn();

        public static bool IsValidPhoneFormat(this string phone) => phone.ToPhoneFormatLength10().Length == 10;

        public static string ToPhoneFormatLength10(this string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "";
            string onlyNumericValue = phone.ToOnlyNumericValue();
            if (onlyNumericValue.Length == 10)
                return onlyNumericValue;
            if (onlyNumericValue.StartsWith("90") && onlyNumericValue.Length == 12)
                return onlyNumericValue.Substring(2, onlyNumericValue.Length - 2);
            return onlyNumericValue.StartsWith("0") && onlyNumericValue.Length == 11 ? onlyNumericValue.Substring(1, onlyNumericValue.Length - 1) : "";
        }

        public static string ToPhoneFormat(this string phone, string format = "5554443322")
        {
            if (!phone.IsValidPhoneFormat())
                return "";
            string phoneFormatLength10 = phone.ToPhoneFormatLength10();
            switch (format)
            {
                case "+90 (555) 444 33 33":
                    return "+90 (" + phoneFormatLength10.Substring(0, 3) + ") " + phoneFormatLength10.Substring(3, 3) + " " + phoneFormatLength10.Substring(6, 2) + " " + phoneFormatLength10.Substring(8, 2);
                case "+90 (555) 444 3333":
                    return "+90 (" + phoneFormatLength10.Substring(0, 3) + ") " + phoneFormatLength10.Substring(3, 3) + " " + phoneFormatLength10.Substring(6, 4);
                case "0 (555) 444 33 22":
                    return "0 (" + phoneFormatLength10.Substring(0, 3) + ") " + phoneFormatLength10.Substring(3, 3) + " " + phoneFormatLength10.Substring(6, 2) + " " + phoneFormatLength10.Substring(8, 2);
                case "0 (555) 444 3322":
                    return "0 (" + phoneFormatLength10.Substring(0, 3) + ") " + phoneFormatLength10.Substring(3, 3) + " " + phoneFormatLength10.Substring(6, 4);
                case "0 555 444 33 22":
                    return "0 " + phoneFormatLength10.Substring(0, 3) + " " + phoneFormatLength10.Substring(3, 3) + " " + phoneFormatLength10.Substring(6, 2) + " " + phoneFormatLength10.Substring(8, 2);
                case "0 555 444 3322":
                    return "0 " + phoneFormatLength10.Substring(0, 3) + " " + phoneFormatLength10.Substring(3, 3) + " " + phoneFormatLength10.Substring(6, 4);
                case "05554443322":
                    return "0" + phoneFormatLength10;
                case "5554443322":
                    return phoneFormatLength10;
                case "90 (555) 444 33 33":
                    return "90 (" + phoneFormatLength10.Substring(0, 3) + ") " + phoneFormatLength10.Substring(3, 3) + " " + phoneFormatLength10.Substring(6, 2) + " " + phoneFormatLength10.Substring(8, 2);
                case "90 (555) 444 3333":
                    return "90 (" + phoneFormatLength10.Substring(0, 3) + ") " + phoneFormatLength10.Substring(3, 3) + " " + phoneFormatLength10.Substring(6, 4);
                default:
                    return phoneFormatLength10;
            }
        }

        public static string ToHtmlTable<T>(this List<T> entities, string cssId = null, string cssClass = null)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table id='" + (string.IsNullOrWhiteSpace(cssId) ? "tableData" : cssId) + "' class='" + (string.IsNullOrWhiteSpace(cssClass) ? "table table-hover" : cssClass) + "'>");
            stringBuilder.Append("<thead>");
            stringBuilder.Append("<tr>");
            Type type = typeof(T);
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.CanRead && property.CanWrite && TypeDescriptor.GetConverter(property.PropertyType).CanConvertFrom(typeof(string)))
                {
                    object[] customAttributes = property.GetCustomAttributes(typeof(HtmlTableColumnAttribute), false);
                    string str = customAttributes == null || customAttributes.Length == 0 ? property.Name : (customAttributes[0] as HtmlTableColumnAttribute).PropertyName;
                    stringBuilder.Append("<th>" + str + "</th>");
                }
            }
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</thead>");
            stringBuilder.Append("<tbody>");
            foreach (T entity in entities)
            {
                stringBuilder.Append("<tr>");
                foreach (PropertyInfo property in type.GetProperties())
                {
                    if (property.CanRead && property.CanWrite && TypeDescriptor.GetConverter(property.PropertyType).CanConvertFrom(typeof(string)))
                    {
                        object obj = property.GetValue((object)entity, (object[])null);
                        stringBuilder.Append(string.Format("<td>{0}</td>", obj));
                    }
                }
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</tbody>");
            stringBuilder.Append("</table>");
            return (string)null;
        }

        public static string ToString(this bool value, string correct, string wrong) => !value ? wrong : correct;

        public static string ToStringJoin<T>(this IEnumerable<T> list, string separator) => list == null || !list.Any<T>() ? "" : string.Join<T>(separator, list);

        public static string TrimWithNullCheck(this string value) => string.IsNullOrWhiteSpace(value) ? (string)null : value.Trim();

        public static List<string> SplitWithNullCheck(this string value, char separator) => string.IsNullOrWhiteSpace(value) ? new List<string>() : ((IEnumerable<string>)value.Split(separator)).ToList<string>();

        public static string ReplaceWithNullCheck(this string value, string oldVal, string newVal) => string.IsNullOrWhiteSpace(value) ? (string)null : value.Replace(oldVal, newVal);

        public static string RemoveEnd(this string value, int length) => string.IsNullOrWhiteSpace(value) || value.Length <= length ? (string)null : value.Substring(0, value.Length - length);

        public static string RemoveStart(this string value, int length) => string.IsNullOrWhiteSpace(value) || value.Length <= length ? (string)null : value.Substring(length);

        public static string Remove(this string value, params string[] arg)
        {
            if (string.IsNullOrWhiteSpace(value))
                return (string)null;
            foreach (string oldValue in arg)
                value = value.Replace(oldValue, "");
            return value;
        }

        public static byte[] ToBytes(this string value) => Encoding.UTF8.GetBytes(value);

        public static string ToCapitalize(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return (string)null;
            return value.Length <= 1 ? value.ToUpper() : value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
        }

        public static string ToCapitalize(this string value, char separator) => string.IsNullOrWhiteSpace(value) ? (string)null : ((IEnumerable<string>)value.Split(separator)).Select<string, string>((Func<string, string>)(s => s.ToCapitalize())).ToStringJoin<string>(" ");

        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        public static bool HasValue(this string value) => !value.IsNullOrWhiteSpace();


        public static string ConvertTurkishToEnglishAlphabet(this string text)
        {
            if (text.IsNullOrWhiteSpace())
                return text;

            var result = new char[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                result[i] = TurkishToEnglishMap.TryGetValue(c, out char replacement) ? replacement : c;
            }
            return new string(result);
        }

        private static readonly Dictionary<char, char> TurkishToEnglishMap = new Dictionary<char, char>
        {
            {'Ç', 'C'}, {'ç', 'c'},
            {'Ğ', 'G'}, {'ğ', 'g'},
            {'İ', 'I'}, {'ı', 'i'},
            {'Ö', 'O'}, {'ö', 'o'},
            {'Ş', 'S'}, {'ş', 's'},
            {'Ü', 'U'}, {'ü', 'u'}
        };


    }
}
