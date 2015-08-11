using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WuWen.Infrastructure.Data
{
    public class UpdateBuilder : InsertAndUpdateBuilderBase<UpdateBuilder>
    {
        public readonly Dictionary<string, IDataParameter[]> clauseDictionary;
        public UpdateBuilder(string tableName) : base(tableName)
        {
            clauseDictionary = new Dictionary<string, IDataParameter[]>();
        }
        public UpdateBuilder RegisterClause(IWhereBuilder whereBuilder)
        {
            if (whereBuilder == null)
            {
                return this;
            }
            Tuple<string, IEnumerable<IDataParameter>> tuple = whereBuilder.Build();
            return RegisterClause(tuple.Item1, tuple.Item2.ToArray<IDataParameter>());
        }
        public UpdateBuilder RegisterClause(string clause, params IDataParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(clause))
            {
                throw new ArgumentNullException("clause");
            }
            if (!clauseDictionary.ContainsKey(clause))
            {
                clauseDictionary.Add(clause, parameters ?? new IDataParameter[0]);
            }
            return this;
        }
        public override Tuple<string, IEnumerable<IDataParameter>> Build()
        {
            string text = string.Format("update {0} set {1} where 1=1", base.TableName, string.Join(",",
                from f in base.Fields
                select string.Format("{0}={1}", f.Key, f.Value.ParameterName)));
            List<IDataParameter> list = new List<IDataParameter>();
            foreach (KeyValuePair<string, IDataParameter[]> current in clauseDictionary)
            {
                text += string.Format(" and {0}", current.Key);
                list.AddRange(current.Value);
            }
            list.AddRange((
                from f in base.Fields
                select f.Value).ToArray<IDataParameter>());
            return new Tuple<string, IEnumerable<IDataParameter>>(text, list);
        }
    }
}
