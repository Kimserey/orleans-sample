using Orleans.Runtime.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansExample
{
    //Silo
    public class Program
    {
        public static void Main(string[] args)
        {
            var silo = new SiloHost("main");
            silo.InitializeOrleansSilo();
            var success = silo.StartOrleansSilo();

            if (!success)
            {
                throw new Exception("Failed to start silo");
            }

            Console.ReadKey();
        }
    }
}
