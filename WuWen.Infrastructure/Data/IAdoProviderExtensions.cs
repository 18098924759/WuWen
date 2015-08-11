using System;
using System.Collections.Generic;
using System.Data;

namespace WuWen.Infrastructure.Data
{
    public static class IAdoProviderExtensions
    {
        
        public static T ExecuteScalar<T>(this IAdoProvider col, string connectionString, string commandText, params IDataParameter[] commandParameters)
        {
            return col.ExecuteScalar<T>(col.BuildConnection(connectionString), commandText, commandParameters);
        }
        public static void ExecuteReader(this IAdoProvider col, Action<IDataReader> readCallback, string connectionString, string commandText, params IDataParameter[] commandParameters)
        {
            col.ExecuteReader(readCallback, col.BuildConnection(connectionString), commandText, commandParameters);
        }
        public static IList<T> ExecuteReader<T>(this IAdoProvider col, Func<IDataReader, T> buildEntity, string connectionString, string commandText, params IDataParameter[] commandParameters)
        {
            return col.ExecuteReader<T>(buildEntity, col.BuildConnection(connectionString), commandText, commandParameters);
        }
        public static int ExecuteNonQuery(this IAdoProvider col, string connectionString, string commandText, params IDataParameter[] commandParameters)
        {
            return col.ExecuteNonQuery(col.BuildConnection(connectionString), commandText, commandParameters);
        }
        public static DataException ExecuteTransaction(this IAdoProvider col, string connectionString, params Action<IDbConnection>[] executeActions)
        {
            return col.ExecuteTransaction(col.BuildConnection(connectionString), executeActions);
        }
    }
}
