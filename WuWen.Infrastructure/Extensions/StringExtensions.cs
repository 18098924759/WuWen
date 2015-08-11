using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WuWen.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        private static Encoding coding = Encoding.GetEncoding("gb2312");
        private static readonly Regex humps = new Regex("(?:^[a-zA-Z][^A-Z]*|[A-Z][^A-Z]*)");
        public static string SubstringByChineseRules(this string col, int length)
        {
            return col.SubstringByChineseRules(0, length);
        }
        public static string SubstringByChineseRules(this string col, int startIndex, int length)
        {
            if (length < 0)
            {
                return string.Empty;
            }
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            byte[] bytes = coding.GetBytes(col);
            int num = bytes.Length - startIndex;
            if (num <= 0)
            {
                return string.Empty;
            }
            if (coding.GetString(bytes, 0, startIndex).EndsWith("?"))
            {
                startIndex++;
            }
            int count = (num >= length) ? length : num;
            return coding.GetString(bytes, startIndex, count).TrimEnd(new char[]
            {
                '?'
            });
        }
        public static string RemoveWhiteSpace(this string col)
        {
            bool flag = false;
            char[] array = col.ToCharArray();
            int num = 0;
            int i = 0;
            int num2 = array.Length;
            while (i < num2)
            {
                char c = array[i];
                if (char.IsWhiteSpace(c))
                {
                    flag = true;
                }
                else
                {
                    array[num] = c;
                    num++;
                }
                i++;
            }
            if (!flag)
            {
                return col;
            }
            return new string(array, 0, num);
        }
        public static string RemoveHtml(this string col)
        {
            return Regex.Replace(col, "<!--([\\s\\S])*?-->|<script([\\s\\S])*?/script>|<[^>]*>|&(nbsp|#160);|([\\r\\n])", "");
        }
        public static string GetLastItemBy(this string col, char split)
        {
            string[] array = col.Split(new char[]
            {
                split
            });
            return array[array.Length - 1];
        }
        public static string GetFirstItemBy(this string col, char split)
        {
            string[] array = col.Split(new char[]
            {
                split
            });
            return array[0];
        }
        public static string CamelFriendly(this string camel)
        {
            if (string.IsNullOrWhiteSpace(camel))
            {
                return "";
            }
            string[] source = (
                from m in humps.Matches(camel).OfType<Match>()
                select m.Value).ToArray();
            if (!source.Any())
            {
                return camel;
            }
            return source.Aggregate((string a, string b) => a + " " + b).TrimStart(new char[]
            {
                ' '
            });
        }
        public static string ToHexString(this string col)
        {
            Func<char, bool> func = delegate (char chr)
            {
                string text = "$-_.+!*'(),@=&";
                return chr > '\u007f' || (!char.IsLetterOrDigit(chr) && text.IndexOf(chr) < 0);
            };
            Func<char, string> func2 = delegate (char chr)
            {
                byte[] bytes = new UTF8Encoding().GetBytes(chr.ToString());
                StringBuilder stringBuilder2 = new StringBuilder();
                byte[] array3 = bytes;
                for (int j = 0; j < array3.Length; j++)
                {
                    byte value = array3[j];
                    stringBuilder2.AppendFormat("%{0}", Convert.ToString(value, 16));
                }
                return stringBuilder2.ToString();
            };
            char[] array = col.ToCharArray();
            StringBuilder stringBuilder = new StringBuilder();
            char[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                char c = array2[i];
                if (func(c))
                {
                    stringBuilder.Append(func2(c));
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
