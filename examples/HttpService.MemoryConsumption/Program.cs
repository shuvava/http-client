using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using HttpService.Abstractions;
using HttpService.Abstractions.Serializers;
using HttpService.Serializers;

using Microsoft.Extensions.DependencyInjection;
// ReSharper disable UnusedMember.Local


namespace HttpService.MemoryConsumption
{
    internal class Program
    {
        const string Url = "https://github.com/tugberkugurlu/ASPNETWebAPISamples/archive/master.zip";
        //const string Url = "https://codeload.github.com/tensorflow/models/zip/master";

        // ReSharper disable once UnusedParameter.Local
        private static async Task Main(string[] args)
        {
            var serviceFactory = ServiceProviderFactory();
            var httpClient = serviceFactory.GetService<IRestClient>();
            long finalByteCount = 0;
            Console.WriteLine("Memory consumption");
            var originalByteCount = GC.GetTotalMemory(true);
            Console.WriteLine($"Original bytes count {originalByteCount:n0}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            await HttpGetForLargeFileInWrongWay().ConfigureAwait(false);
            //await HttpServiceLibrary(httpClient).ConfigureAwait(false);
            //await HttpGetForLargeFileInRightWay().ConfigureAwait(false);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        private static async Task HttpGetForLargeFileInWrongWay()
        {
            Console.WriteLine($"    using {nameof(HttpGetForLargeFileInWrongWay)}");
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(Url))
                {
                    var data = await response.Content.ReadAsStringAsync();

                    var fileToWriteTo = Path.GetTempFileName();

                    using (var streamToWriteTo = File.Open(fileToWriteTo, FileMode.Create))
                    {
                        var bytes = Encoding.ASCII.GetBytes(data);
                        await streamToWriteTo.WriteAsync(bytes, 0, bytes.Length);
                        //streamToWriteTo.WriteAsync(streamToReadFrom)
                    }

                    response.Content = null;
                }
            }
            var finalByteCount = GC.GetTotalMemory(true);
            Console.WriteLine($"Final bytes count {finalByteCount:n0}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static async Task HttpGetForLargeFileInRightWay()
        {
            Console.WriteLine($"    using {nameof(HttpGetForLargeFileInRightWay)}");
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(Url, HttpCompletionOption.ResponseHeadersRead))
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    string fileToWriteTo = Path.GetTempFileName();
                    using (Stream streamToWriteTo = File.Open(fileToWriteTo, FileMode.Create))
                    {
                        await streamToReadFrom.CopyToAsync(streamToWriteTo);
                    }
                }
            }
            var finalByteCount = GC.GetTotalMemory(true);
            Console.WriteLine($"Final bytes count {finalByteCount:n0}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        private static async Task HttpServiceLibrary(IRestClient httpClient)
        {
            Console.WriteLine($"    using {nameof(HttpServiceLibrary)}");
            using (var streamToReadFrom = await httpClient.GetAsync<Stream>(Url))
            {
                string fileToWriteTo = Path.GetTempFileName();
                using (Stream streamToWriteTo = File.Open(fileToWriteTo, FileMode.Create))
                {
                    await streamToReadFrom.CopyToAsync(streamToWriteTo);
                }
            }
            var finalByteCount = GC.GetTotalMemory(true);
            Console.WriteLine($"Final bytes count {finalByteCount:n0}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static IServiceProvider ServiceProviderFactory()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ISerializer, JsonRestSerializer>();
            services.AddHttpClient<IRestClient, RestClient>();

            return services.BuildServiceProvider();
        }
    }
}
