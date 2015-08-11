using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuWen.Infrastructure.Data;

namespace ConsoleApplication1
{
    public abstract class RepositoryBase
    {
        protected IAdoProvider AdoProvide { get; set; }

        protected string ReadConnectionString { get; set; }

        protected string WriteConnectionString { get; private set; }

        protected RepositoryBase(IAdoProvider adoProvider)
        {
            AdoProvide = adoProvider;
            WriteConnectionString = ConfigurationManager.ConnectionStrings["opm_write"].ConnectionString;
            ReadConnectionString = ConfigurationManager.ConnectionStrings["opm_read"].ConnectionString; 
        } 
    }
}
