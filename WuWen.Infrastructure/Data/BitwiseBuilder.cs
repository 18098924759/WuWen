using System;
using System.Collections.Generic;
using System.Data;

namespace WuWen.Infrastructure.Data
{
    public class BitwiseBuilder : WhereBuilderBase
    {
        private readonly int? value;
        public BitwiseBuilder(string fieldName, int? value) : base(fieldName)
        {
            this.value = value;
        }
        public override Tuple<string, IEnumerable<IDataParameter>> Build()
        {
            if (!value.HasValue)
            {
                return null;
            }
            string item = string.Format("{0}={1}&{0}", FieldName, BuildParameterName(FieldName));
            List<IDataParameter> item2 = new List<IDataParameter>
            {
                base.BuildParameter<int?>(FieldName, value)
            };
            return new Tuple<string, IEnumerable<IDataParameter>>(item, item2);
        }
    }
}
