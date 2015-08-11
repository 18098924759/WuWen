using System;
using System.Collections.Generic;
using System.Data;

namespace WuWen.Infrastructure.Data
{
    public class EqualBuilder<T> : WhereBuilderBase
    {
        private readonly T value;
        public EqualBuilder(string fieldName, T value) : base(fieldName)
        {
            this.value = value;
        }
        public override Tuple<string, IEnumerable<IDataParameter>> Build()
        {
            if (value == null)
            {
                return null;
            }
            return new Tuple<string, IEnumerable<IDataParameter>>(string.Format("{0}={1}", FieldName, BuildParameterName(FieldName)), new List<IDataParameter>
            {
                base.BuildParameter<T>(FieldName, value)
            });
        }
    }
}
