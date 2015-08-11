using System;
using System.Collections.Generic;
using System.Data;

namespace WuWen.Infrastructure.Data
{
    public abstract class WhereBuilderBase : IWhereBuilder
    {
        protected readonly IAdoProvider adoPrivate;
        protected string FieldName
        {
            get;
            private set;
        }
        protected WhereBuilderBase(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentNullException("fieldName");
            }
            adoPrivate = new MySqlAdoProvider();
            FieldName = fieldName;
        }
        protected string BuildParameterName(string fieldName)
        {
            return adoPrivate.BuildParameterName(fieldName);
        }
        protected IDataParameter BuildParameter<T>(string fieldName, T value)
        {
            return adoPrivate.BuildParameter<T>(fieldName, value);
        }
        public abstract Tuple<string, IEnumerable<IDataParameter>> Build();
    }
}
