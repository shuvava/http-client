using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;


namespace HttpService.ConnectionLimit_netCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var timer = Stopwatch.StartNew();
            RunCocurren_ConnectionsWithConcurrency2();
            //RunCocurren();
            timer.Stop();
            Console.WriteLine($"Time taken {timer.ElapsedMilliseconds:N0}ms");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void RunCocurren()
        {
            var handler = new HttpClientHandler();
            //by default handler.MaxConnectionsPerServer = int.MaxValue.
            //handler.MaxConnectionsPerServer = 2;

            #region AdvansedSettings
            //var handler = new SocketsHttpHandler();
            //handler.PooledConnectionLifetime = TimeSpan.FromSeconds(60); //analogue ConnectionLeaseTimeout
            //handler.PooledConnectionIdleTimeout = TimeSpan.FromSeconds(100); //analogue MaxIdleTime

            //var client = new HttpClient(handler);
            #endregion

            using (var client = new HttpClient(handler))
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
        static void RunCocurren_ConnectionsWithConcurrency2()
        {
            var handler = new HttpClientHandler();
            handler.MaxConnectionsPerServer = 2;
            using (var client = new HttpClient(handler))
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
