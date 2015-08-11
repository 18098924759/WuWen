using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuWen.Infrastructure.Data;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var testRepository = new TestRepository(new MySqlAdoProvider());
            Console.WriteLine(testRepository.GetUserCount());
            Console.ReadKey();
        }
    }
}
