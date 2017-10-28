using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrleansExample
{
    //Silo
    public class Program
    {
        public static void Main(string[] args)
        {
            var primaryEndpoint = new IPEndPoint(IPAddress.Loopback, Int32.Parse(args[2]));
            var siloEndpoint = new IPEndPoint(IPAddress.Loopback, Int32.Parse(args[0]));
            var gatewayEntpoint = new IPEndPoint(IPAddress.Loopback, Int32.Parse(args[1]));

            var silo = new SiloHost(Dns.GetHostName() + "@" + args[0]);
            silo.LoadOrleansConfig();
            silo.Config.Globals.DeploymentId = "main";
            silo.SetProxyEndpoint(gatewayEntpoint);
            silo.SetSiloEndpoint(siloEndpoint, 0);
            silo.SetPrimaryNodeEndpoint(primaryEndpoint);
            silo.SetSeedNodeEndpoint(primaryEndpoint);
            silo.InitializeOrleansSilo();

            var success = silo.StartOrleansSilo();

            if (!success)
            {
                throw new Exception("Failed to start silo");
            }

            Console.ReadLine();
        }
    }
}

