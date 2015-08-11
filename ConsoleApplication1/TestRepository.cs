using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuWen.Infrastructure.Data;

namespace ConsoleApplication1
{
    public class TestRepository : RepositoryBase
    {
        public TestRepository(IAdoProvider adoProvider) : base(adoProvider)
        {
        }


        public int GetUserCount()
        {
            var query = new SelectBuilder("select count(1) from abpusers");
            return AdoProvide.ExecuteScalar<int>(WriteConnectionString, query);
        }
    }
}
