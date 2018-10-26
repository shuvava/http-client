using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using HttpService.Abstractions;
using HttpService.Abstractions.Serializers;
using HttpService.Serializers;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;


namespace HttpService.ExampleWithPolly
{
    class Program
    {
        //private const string url = "https://reqres.in/api/users";
        private const string url = "http://slowwly.robertomurray.co.uk/delay/5000/url/https://reqres.in/api/users";
        static async Task Main(string[] args)
        {
            var serviceFactory = ServiceProviderFactory();
            var httpClient = serviceFactory.GetService<IRestClient>();

            try
            {
                var response = await httpClient.GetAsync<Response<User>>(url).ConfigureAwait(false);

                if (response != null)
                {
                    foreach (var user in response.Data)
                    {
                        Console.WriteLine($"FirstName: {user.FirstName}; LastName:{user.LastName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Global handler: Error");
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        private static IServiceProvider ServiceProviderFactory()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ISerializer, JsonRestSerializer>();

            //https://github.com/App-vNext/Polly
            // set custom timeout

            //var retryWithTimeout = retryPolicy.Wrap(timeoutPolicy);
            var itrattionNum = 0;
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>() // thrown by Polly's TimeoutPolicy if the inner call times out
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                }, (result, span) =>
                {
                    Console.WriteLine("Timeout => Retry");
                });

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(() => TimeSpan.FromSeconds(4+ itrattionNum++));

            //https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory
            services
                .AddHttpClient<IRestClient, RestClient>()
                .AddPolicyHandler(retryPolicy)
                .AddPolicyHandler(timeoutPolicy);

            return services.BuildServiceProvider();
        }
        private class Response<T>
        {
            public int Page { get; set; }
            public int Total { get; set; }
            public IEnumerable<T> Data { get; set; }
        }


        private class User
        {
            public int Id { get; set; }


            [JsonProperty("first_name")]
            public string FirstName { get; set; }


            [JsonProperty("last_name")]
            public string LastName { get; set; }


            public string Avatar { get; set; }
        }
    }
}
