using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WuWen.Infrastructure.Data
{
    public class InsertBuilder : InsertAndUpdateBuilderBase<InsertBuilder>
    {
        public bool IsReturnId
        {
            get;
            set;
        }
        public InsertBuilder(string tableName, bool isReturnId = true) : base(tableName)
        {
            IsReturnId = isReturnId;
        }
        public override Tuple<string, IEnumerable<IDataParameter>> Build()
        {
            string arg_7F_0 = "insert into {0}({1}) values({2});{3}";
            object[] array = new object[4];
            array[0] = base.TableName;
            array[1] = string.Join(",", base.Fields.Keys);
            array[2] = string.Join(",",
                from item in base.Fields.Values
                select item.ParameterName);
            array[3] = (IsReturnId ? "select @@Identity;" : string.Empty);
            string item2 = string.Format(arg_7F_0, array);
            return new Tuple<string, IEnumerable<IDataParameter>>(item2, base.Fields.Values);
        }
    }
}
