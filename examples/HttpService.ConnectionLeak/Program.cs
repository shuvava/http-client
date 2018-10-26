using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace HttpService.ConnectionLeak
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // netstat -n | select-string -pattern "178.248.237.68"
            Console.WriteLine("Memory leak example");
            //ConnectionLeak().Wait();
            Singletone().Wait();
            Console.WriteLine("Getting Current Connections");
            var ports = NetStat.GetNetStatPorts();

            foreach (var port in ports.Where(w => w.remote_address.Contains("178.248.237.68")))
            {
                Console.WriteLine(
                    $"process_name:'{port.process_name}' protocol:{port.protocol} status:{port.status} remote_address:{port.remote_address}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        private static async Task ConnectionLeak()
        {
            for (var i = 0; i < 10; i++)
            {
                using (var client = new HttpClient())
                {
                    await client.GetStringAsync("https://habr.com").ConfigureAwait(false);
                }
            }
        }


        private static async Task Singletone()
        {
            using (var client = new HttpClient())
            {
                for (var i = 0; i < 10; i++)
                {
                    await client.GetStringAsync("https://habr.com").ConfigureAwait(false);
                }
            }
        }
    }
}
