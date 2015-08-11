using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WuWen.Infrastructure.Data
{
    public class InBuilder<T> : WhereBuilderBase
    {
        private readonly IEnumerable<T> source;
        public InBuilder(string fieldName, IEnumerable<T> col) : base(fieldName)
        {
            source = (col ?? Enumerable.Empty<T>());
        }
        public override Tuple<string, IEnumerable<IDataParameter>> Build()
        {
            if (!source.Any<T>())
            {
                return null;
            }
            List<IDataParameter> list = new List<IDataParameter>();
            for (int i = 0; i < source.Count<T>(); i++)
            {
                list.Add(base.BuildParameter<T>(string.Format("{0}_{1}", FieldName, i), source.ElementAt(i)));
            }
            string item2 = string.Format("{0} in ({1})", FieldName, string.Join(",",
                from item in list
                select item.ParameterName));
            return new Tuple<string, IEnumerable<IDataParameter>>(item2, list);
        }
    }
}
