using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WuWen.Infrastructure.Data;
using WuWen.Infrastructure.Extensions;

namespace ConsoleApplication1
{
    public class MySqlAdoProvider : IAdoProvider
    {
        public T ExecuteScalar<T>(IDbConnection connection, string commandText, params IDataParameter[] commandParameters)
        {
            return Execute(connection, (MySqlConnection conn) => MySqlHelper.ExecuteScalar(conn, commandText, ToMySqlParameters(commandParameters)).As<T>());
        }
        public void ExecuteReader(Action<IDataReader> readCallback, IDbConnection connection, string commandText, params IDataParameter[] commandParameters)
        {
            Execute(connection, delegate (MySqlConnection conn)
            {
                using (MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(conn, commandText, ToMySqlParameters(commandParameters)))
                {
                    while (mySqlDataReader.Read())
                    {
                        readCallback(mySqlDataReader);
                    }
                }
            });
        }
        public IList<T> ExecuteReader<T>(Func<IDataReader, T> buildEntity, IDbConnection connection, string commandText, params IDataParameter[] commandParameters)
        {
            if (buildEntity == null)
            {
                throw new ArgumentNullException("buildEntity");
            }
            List<T> list = new List<T>();
            Execute(connection, delegate (MySqlConnection conn)
            {
                using (MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(conn, commandText, ToMySqlParameters(commandParameters)))
                {
                    while (mySqlDataReader.Read())
                    {
                        list.Add(buildEntity(mySqlDataReader));
                    }
                }
            });
            return list;
        }
        public int ExecuteNonQuery(IDbConnection connection, string commandText, params IDataParameter[] commandParameters)
        {
            return Execute(connection, (MySqlConnection conn) => MySqlHelper.ExecuteNonQuery(conn, commandText, ToMySqlParameters(commandParameters)));
        }
        public DataException ExecuteTransaction(IDbConnection connection, params Action<IDbConnection>[] executeActions)
        {
            if (executeActions == null || executeActions.Length == 0)
            {
                return null;
            }
            DataException result;
            using (IDbTransaction dbTransaction = connection.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < executeActions.Length; i++)
                    {
                        Action<IDbConnection> action = executeActions[i];
                        action(connection);
                    }
                    dbTransaction.Commit();
                    result = null;
                }
                catch (DataException ex)
                {
                    dbTransaction.Rollback();
                    result = ex;
                }
            }
            return result;
        }
        public IDataParameter BuildParameter<T>(string name, T value)
        {
            string text = BuildParameterName(name);
            if (value != null)
            {
                return new MySqlParameter(text, value);
            }
            return new MySqlParameter(text, DBNull.Value);
        }
        public string BuildParameterName(string name)
        {
            return string.Format("@{0}", name.TrimStart(new char[]
            {
                '@'
            }).Replace(".", ""));
        }
        public IDbConnection BuildConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
        private MySqlConnection toMySqlConnection(IDbConnection connection)
        {
            return connection as MySqlConnection;
        }
        private MySqlParameter[] ToMySqlParameters(IDataParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return new MySqlParameter[0];
            }
            return parameters.OfType<MySqlParameter>().ToArray<MySqlParameter>();
        }
        private void Execute(IDbConnection connection, Action<MySqlConnection> action)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            action(toMySqlConnection(connection));
            connection.Close();
        }
        private T Execute<T>(IDbConnection connection, Func<MySqlConnection, T> action)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            T result = action(toMySqlConnection(connection));
            connection.Close();
            return result;
        }
    }
}
