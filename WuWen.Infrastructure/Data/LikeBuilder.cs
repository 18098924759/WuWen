using System;
using System.Collections.Generic;
using System.Data;

namespace WuWen.Infrastructure.Data
{
    public class LikeBuilder : WhereBuilderBase
    {
        public enum LikeMode
        {
            All,
            Left,
            Rigth
        }
        private readonly string value;
        private readonly LikeBuilder.LikeMode mode;
        public LikeBuilder(string fieldName, string value, LikeBuilder.LikeMode mode = LikeBuilder.LikeMode.All) : base(fieldName)
        {
            this.value = value;
            this.mode = mode;
        }
        public override Tuple<string, IEnumerable<IDataParameter>> Build()
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            List<IDataParameter> item = new List<IDataParameter>
            {
                base.BuildParameter<string>(FieldName, value)
            };
            switch (mode)
            {
                case LikeBuilder.LikeMode.Left:
                    return new Tuple<string, IEnumerable<IDataParameter>>(string.Format("{0} like concat('%',{1})", FieldName, BuildParameterName(FieldName)), item);
                case LikeBuilder.LikeMode.Rigth:
                    return new Tuple<string, IEnumerable<IDataParameter>>(string.Format("{0} like concat({1},'%')", FieldName, BuildParameterName(FieldName)), item);
                default:
                    return new Tuple<string, IEnumerable<IDataParameter>>(string.Format("{0} like concat('%',{1},'%')", FieldName, BuildParameterName(FieldName)), item);
            }
        }
    }
}
