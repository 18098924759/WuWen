using System;
using System.Data;
using System.Linq;

namespace WuWen.Infrastructure.Extensions
{
    public static class DataReaderExtensions
    {
        public static bool ContainsColumn(this IDataReader col, string columnName)
        {
            DataTable schemaTable = col.GetSchemaTable();
            return schemaTable != null && schemaTable.Rows.Cast<DataRow>().Any((DataRow row) => row != null && string.Equals(row["ColumnName"].ToString(), columnName, StringComparison.CurrentCultureIgnoreCase));
        }
        public static T Get<T>(this IDataReader col, string columnName)
        {
            return col[columnName].As<T>();
        }
        public static T Get<T>(this IDataReader col, string columnName, T defaultValue)
        {
            if (!col.ContainsColumn(columnName))
            {
                return defaultValue;
            }
            return col[columnName].As(defaultValue);
        }
    }
}
