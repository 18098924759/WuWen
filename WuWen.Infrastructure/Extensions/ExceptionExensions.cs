using System;

namespace WuWen.Infrastructure.Extensions
{
    public static class ExceptionExensions
    {
        public static void ArgumentNullException(this string col, string argName, string message = null)
        {
            if (string.IsNullOrWhiteSpace(col))
            {
                throw new ArgumentNullException(message ?? string.Format("参数\"{0}\"不能为null或empty。", argName));
            }
        }
        public static void ArgumentNullException(this object col, string argName, string message = null)
        {
            if (col == null)
            {
                throw new ArgumentNullException(message ?? string.Format("参数\"{0}\"不能为null。", argName));
            }
        }
    }
}
