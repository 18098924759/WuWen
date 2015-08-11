using System;
using System.Collections.Generic;
using System.Data;

namespace WuWen.Infrastructure.Data
{
    public abstract class InsertAndUpdateBuilderBase<TSqlBuilder> : ISqlBuilder where TSqlBuilder : InsertAndUpdateBuilderBase<TSqlBuilder>
    {
        protected string TableName
        {
            get;
            private set;
        }
        protected Dictionary<string, IDataParameter> Fields
        {
            get;
            private set;
        }
        private IAdoProvider AdoProvide
        {
            get;
            set;
        }
        protected InsertAndUpdateBuilderBase(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }
            TableName = tableName;
            AdoProvide = new MySqlAdoProvider();
            Fields = new Dictionary<string, IDataParameter>();
        }
        public TSqlBuilder RegisterField<T>(string fieldName, T value, string parameterName = "")
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentNullException("fieldName");
            }
            Fields.Add(fieldName, AdoProvide.BuildParameter<T>(string.IsNullOrWhiteSpace(parameterName) ? fieldName : parameterName, value));
            return this as TSqlBuilder;
        }
        public abstract Tuple<string, IEnumerable<IDataParameter>> Build();
    }
}
