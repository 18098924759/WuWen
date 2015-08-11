using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WuWen.Infrastructure.Extensions
{
    public static class ConvertibleExtensions
    {
        public class StringAsMaps
        {
            public static readonly IDictionary<Type, Func<string, object>> Maps;
            static StringAsMaps()
            {
                Maps = new ConcurrentDictionary<Type, Func<string, object>>();
                Maps.Add(typeof(Guid), (string item) => Guid.Parse(item));
                Maps.Add(typeof(Uri), (string item) => new Uri(item));
                Maps.Add(typeof(TimeSpan), (string item) => TimeSpan.Parse(item));
                Maps.Add(typeof(Type), (string item) => Type.GetType(item));
                Maps.Add(typeof(DateTimeOffset), (string item) => DateTimeOffset.Parse(item));
                Maps.Add(typeof(bool), delegate (string item)
                {
                    int num;
                    if (int.TryParse(item, out num))
                    {
                        return num > 0;
                    }
                    return Convert.ToBoolean(item);
                });
                Maps.Add(typeof(DateTime), delegate (string item)
                {
                    if (item.IsInteger() && (item.Length == 4 || item.Length == 6 || item.Length == 8))
                    {
                        item = item.Insert(4, "-");
                        if (item.Length > 6)
                        {
                            item = item.Insert(7, "-");
                        }
                        else
                        {
                            item += "01-";
                        }
                        if (item.Length <= 8)
                        {
                            item += "01";
                        }
                    }
                    return DateTime.Parse(item);
                });
            }
        }
        public static T As<T>(this object col)
        {
            return (T)((object)col.As(typeof(T), () => default(T)));
        }
        public static T As<T>(this object col, T defaultValue)
        {
            return (T)((object)col.As(typeof(T), defaultValue));
        }
        public static object As(this object col, Type targetType)
        {
            return col.As(targetType, () => BuildDefaultValue(targetType));
        }
        public static object As(this object col, Type targetType, object defaultValue)
        {
            return col.As(targetType, () => defaultValue);
        }
        private static object As(this object col, Type targetType, Func<object> acquireDefaultValue)
        {
            if (col == null || col == DBNull.Value || targetType == null)
            {
                return acquireDefaultValue();
            }
            Type type = col.GetType();
            Type type2 = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (type == type2 || type.IsSubclassOf(type2) || (type2.IsInterface && type.GetInterface(type2.Name) != null))
            {
                return col;
            }
            if (type2 == typeof(string))
            {
                return col.ToString();
            }
            if (type2.IsEnum)
            {
                return Enum.Parse(type2, col.ToString(), true);
            }
            if (col is string)
            {
                return ((string)col).As(type2, acquireDefaultValue);
            }
            object result;
            try
            {
                result = Convert.ChangeType(col, type2);
            }
            catch
            {
                result = acquireDefaultValue();
            }
            return result;
        }
        public static object As(this string col, Type targetType, Func<object> acquireDefaultValue)
        {
            object result;
            try
            {
                result = (StringAsMaps.Maps.ContainsKey(targetType) ? StringAsMaps.Maps[targetType](col) : Convert.ChangeType(col, targetType));
            }
            catch (FormatException)
            {
                result = acquireDefaultValue();
            }
            return result;
        }
        private static object BuildDefaultValue(Type targetType)
        {
            if (targetType == typeof(string))
            {
                return string.Empty;
            }
            object result;
            try
            {
                result = Activator.CreateInstance(targetType);
            }
            catch (MissingMethodException)
            {
                result = null;
            }
            return result;
        }
    }
}
