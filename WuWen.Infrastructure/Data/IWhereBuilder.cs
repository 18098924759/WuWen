using System;
using System.Collections.Generic;
using System.Data;

namespace WuWen.Infrastructure.Data
{
    public interface IWhereBuilder
    {
        Tuple<string, IEnumerable<IDataParameter>> Build();
    }
}
