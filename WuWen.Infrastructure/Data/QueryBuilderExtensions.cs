using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WuWen.Infrastructure.Data
{
    public static class QueryBuilderExtensions
    {
        public static SelectBuilder RegisterClause(this SelectBuilder col, IWhereBuilder clause)
        {
            Tuple<string, IEnumerable<IDataParameter>> tuple = clause.Build();
            if (tuple == null)
            {
                return col;
            }
            return col.RegisterClause(tuple.Item1, (tuple.Item2 == null) ? new IDataParameter[0] : tuple.Item2.ToArray<IDataParameter>());
        }
        public static SelectBuilder RegisterEqualClause<T>(this SelectBuilder col, string fieldName, T value)
        {
            return col.RegisterClause(new EqualBuilder<T>(fieldName, value));
        }
        public static SelectBuilder RegisterLikeClause(this SelectBuilder col, string fieldName, string value, LikeBuilder.LikeMode mode = LikeBuilder.LikeMode.All)
        {
            return col.RegisterClause(new LikeBuilder(fieldName, value, mode));
        }
        public static SelectBuilder RegisterInClause<T>(this SelectBuilder col, string fieldName, IEnumerable<T> value)
        {
            return col.RegisterClause(new InBuilder<T>(fieldName, value));
        }
        public static SelectBuilder RegisterBitwiseClause(this SelectBuilder col, string fieldName, int? value)
        {
            return col.RegisterClause(new BitwiseBuilder(fieldName, value));
        }
    }
}
