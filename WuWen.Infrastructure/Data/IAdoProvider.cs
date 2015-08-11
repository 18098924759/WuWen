using System;
using System.Collections.Generic;
using System.Data;

namespace WuWen.Infrastructure.Data
{
    public interface IAdoProvider
    {
        T ExecuteScalar<T>(IDbConnection connection, string commandText, params IDataParameter[] commandParameters);
        void ExecuteReader(Action<IDataReader> readCallback, IDbConnection connection, string commandText, params IDataParameter[] commandParameters);
        IList<T> ExecuteReader<T>(Func<IDataReader, T> buildEntity, IDbConnection connection, string commandText, params IDataParameter[] commandParameters);
        int ExecuteNonQuery(IDbConnection connection, string commandText, params IDataParameter[] commandParameters);
        DataException ExecuteTransaction(IDbConnection connection, params Action<IDbConnection>[] executeActions);
        IDataParameter BuildParameter<T>(string name, T value);
        string BuildParameterName(string name);
        IDbConnection BuildConnection(string connectionString);
    }
}
