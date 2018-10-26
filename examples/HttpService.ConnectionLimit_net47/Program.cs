using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace HttpService.ConnectionLimit_net47
{
    class Program
    {
        static void Main(string[] args)
        {
            // default restriction to count of concurrent session = 2
            //ServicePointManager.DefaultConnectionLimit = 5;

            #region BaseSettings
            // it is possible to setup connection limit per host
            //var delayServicePoint = ServicePointManager.FindServicePoint(new Uri("http://slowwly.robertomurray.co.uk"));
            //delayServicePoint.ConnectionLimit = 3;
            //var habrServicePoint = ServicePointManager.FindServicePoint(new Uri("https://habr.com"));
            //habrServicePoint.ConnectionLimit = 5;
            #endregion
            #region AdvancedSettings
            //ServicePointManager.DnsRefreshTimeout = 120000; // ip address caching 2 minutes <-> default(120000)
            //var habrServicePoint = ServicePointManager.FindServicePoint(new Uri("https://habr.com"));
            //habrServicePoint.MaxIdleTime = 100000; //idle time of connection; default 100 seconds (100000) 
            //habrServicePoint.ConnectionLeaseTimeout = 60000; // how long connection will open default = -1; =0 connection close immediately  after finish request
            #endregion

            var timer = Stopwatch.StartNew();
            RunCocurrenConnections();
            timer.Stop();
            Console.WriteLine($"Time taken {timer.ElapsedMilliseconds:N0}ms");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        static void RunCocurrenConnections()
        {
            using (var client = new HttpClient())
            {
                var tasks = new List<Task>();

                for (var i = 0; i < 10; i++)
                {
                    // slowdown response for 5 seconds
                    tasks.Add(SendRequest(client, "http://slowwly.robertomurray.co.uk/delay/5000/url/https://habr.com"));
                }

                Task.WaitAll(tasks.ToArray());
            }
        }

        private static async Task SendRequest(HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            Console.WriteLine($"Received response {response.StatusCode} from {url}");
        }
    }
}
