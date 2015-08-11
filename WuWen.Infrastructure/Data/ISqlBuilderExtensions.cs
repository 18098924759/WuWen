using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WuWen.Infrastructure.Data
{
    public static class ISqlBuilderExtensions
    {
        public static int ExecuteNonQuery(this IAdoProvider col, string connectionString, ISqlBuilder sqlBuilder)
        {
            Tuple<string, IEnumerable<IDataParameter>> tuple = sqlBuilder.Build();
            return col.ExecuteNonQuery(connectionString, tuple.Item1, tuple.Item2.ToArray());
        }
        public static void ExecuteReader(this IAdoProvider col, string connectionString, ISqlBuilder sqlBuilder, Action<IDataReader> readCallback)
        {
            Tuple<string, IEnumerable<IDataParameter>> tuple = sqlBuilder.Build();
            col.ExecuteReader(readCallback, connectionString, tuple.Item1, (tuple.Item2 == null) ? new IDataParameter[0] : tuple.Item2.ToArray());
        }
        public static IEnumerable<TEntity> ExecuteReader<TEntity>(this IAdoProvider col, string connectionString, ISqlBuilder sqlBuilder, Func<IDataReader, TEntity> buildEntity)
        {
            Tuple<string, IEnumerable<IDataParameter>> tuple = sqlBuilder.Build();
            return col.ExecuteReader(buildEntity, connectionString, tuple.Item1, tuple.Item2.ToArray());
        }
        public static T ExecuteScalar<T>(this IAdoProvider col, string connectionString, ISqlBuilder sqlBuilder)
        {
            Tuple<string, IEnumerable<IDataParameter>> tuple = sqlBuilder.Build();
            return col.ExecuteScalar<T>(connectionString, tuple.Item1, tuple.Item2.ToArray());
        }
    }
}
