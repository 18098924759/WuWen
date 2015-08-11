using System;
using System.Data;

namespace WuWen.Infrastructure.Data
{
    public class ConnectionPool : IDisposable
    {
        public ConnectionPool(Func<string, IDbConnection> buildConnection)
        {
            if (buildConnection == null)
            {
                throw new ArgumentNullException("buildConnection");
            }
        }
        public IDbConnection GetConnection(string connectionString)
        {
            return null;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
