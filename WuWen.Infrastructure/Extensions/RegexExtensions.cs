using System.Text.RegularExpressions;

namespace WuWen.Infrastructure.Extensions
{
    public static class RegexExtensions
    {
        public static bool IsInteger(this string input)
        {
            return input.IsMatch("^[0-9]+$");
        }
        public static bool IsSignedInteger(this string input)
        {
            return input.IsMatch("^[+-]?[0-9]+$");
        }
        public static bool IsDecimal(this string input)
        {
            return input.IsMatch("^[0-9]+[.]?[0-9]+$");
        }
        public static bool IsSignedDecimal(this string input)
        {
            return input.IsMatch("^[+-]?[0-9]+[.]?[0-9]+$");
        }
        public static bool IsLetter(this string input)
        {
            return input.IsMatch("^[a-zA-Z]+$");
        }
        public static bool IsChinese(this char input)
        {
            return input.ToString().IsMatch("^[一-龥]$");
        }
        public static bool IsEmail(this string input)
        {
            return input.IsMatch("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
        }
        public static bool IsUrl(this string input)
        {
            return input.IsMatch("^[a-zA-z]+://(\\w+(-\\w+)*)(\\.(\\w+(-\\w+)*))*(\\?\\S*)?$");
        }
        public static bool IsChineseMobilePhone(this string input)
        {
            return input.IsMatch("^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\\d{8}$");
        }
        public static bool IsIP(string input)
        {
            return input.IsMatch("^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$");
        }
        public static bool IsUrlSegment(this string segment)
        {
            return Regex.IsMatch(segment, "^[^/?#[\\]@\"^{}|`<>\\s]+$");
        }
        public static bool IsMatch(this string input, string pattern)
        {
            return input.IsMatch(pattern, RegexOptions.None);
        }
        public static bool IsMatch(this string input, string pattern, RegexOptions options)
        {
            return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, pattern, options);
        }
    }
}
