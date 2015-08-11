using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WuWen.Infrastructure.Data
{
    public class SelectBuilder : ISqlBuilder
    {
        private readonly string baseQuery;
        private string orderBy;
        private string groupBy;
        private int pageSize = 9999;
        private int pageIndex;
        public readonly Dictionary<string, IDataParameter[]> clauseDictionary;
        public readonly IEnumerable<IDataParameter> defaultParameters;
        public SelectBuilder(string baseQuery, IEnumerable<IDataParameter> parameters = null)
        {
            orderBy = string.Empty;
            groupBy = string.Empty;
            this.baseQuery = baseQuery.Trim(new char[]
            {
                ';'
            }).ToLower();
            clauseDictionary = new Dictionary<string, IDataParameter[]>();
            defaultParameters = (parameters ?? Enumerable.Empty<IDataParameter>());
        }
        public SelectBuilder RegisterClause(string clause, params IDataParameter[] parameters)
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
        public SelectBuilder RegisterOrderBy(string orderBy)
        {
            this.orderBy = orderBy;
            return this;
        }
        public SelectBuilder RegisterGroupBy(string groupBy)
        {
            this.groupBy = groupBy;
            return this;
        }
        public SelectBuilder RegisterPagination(int pageSize = 20, int pageIndex = 0)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            return this;
        }
        public Tuple<string, IEnumerable<IDataParameter>> Build()
        {
            string text = baseQuery;
            if (clauseDictionary.Count > 0 && baseQuery.IndexOf("where", StringComparison.Ordinal) == -1)
            {
                text = string.Format("{0} where 1=1", baseQuery);
            }
            List<IDataParameter> list = new List<IDataParameter>(defaultParameters);
            foreach (KeyValuePair<string, IDataParameter[]> current in clauseDictionary)
            {
                text += string.Format(" and {0}", current.Key);
                list.AddRange(current.Value);
            }
            if (!string.IsNullOrWhiteSpace(groupBy))
            {
                text += string.Format(" group by {0}", groupBy.ToLower().Replace("order by", ""));
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                text += string.Format(" order by {0}", orderBy.ToLower().Replace("order by", ""));
            }
            text += string.Format(" Limit {0},{1}", pageIndex * pageSize, pageSize);
            return new Tuple<string, IEnumerable<IDataParameter>>(text, list);
        }
    }
}
