using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using HttpService.Abstractions;
using HttpService.Abstractions.Serializers;
using HttpService.Serializers;


namespace HttpService.Example
{
    internal class Program
    {
        private const string BaseUrl = "https://reqres.in";


        private static async Task Main(string[] args)
        {
            var serviceFactory = ServiceProviderFactory();
            var httpClient = serviceFactory.GetService<IRestClient>();
            var url = HttpUtils.JoinUrls(BaseUrl, "/api/users");
            var response = await httpClient.GetAsync<Response<User>>(url).ConfigureAwait(false);

            foreach (var user in response.Data)
            {
                Console.WriteLine($"FirstName: {user.FirstName}; LastName:{user.LastName}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        private static IServiceProvider ServiceProviderFactory()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ISerializer, JsonRestSerializer>();
            services.AddHttpClient<IRestClient, RestClient>();

            #region custom_factory
            //services.AddHttpClient<IRestClient, RestClient>(client =>
            //{
            //    client.BaseAddress = new Uri("https://api.github.com/");
            //    client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            //    client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactoryTesting");
            //});
            #endregion

            #region custom_factory_named
            // custom factory named
            //services.AddHttpClient<IRestClient, RestClient>("GitHubClient", client =>
            //{
            //    client.BaseAddress = new Uri("https://api.github.com/");
            //    client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            //    client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactoryTesting");
            //});

            //usage
            /***
            public class ValuesController : Controller
            {
                private readonly IHttpClientFactory _httpClientFactory;

                public ValuesController(IHttpClientFactory httpClientFactory)
                {
                    _httpClientFactory = httpClientFactory;
                }

                [HttpGet]
                public async Task<ActionResult> Get()
                {
                    var client = _httpClientFactory.CreateClient("GitHubClient");
                    var result = await client.GetStringAsync("/");

                    return Ok(result);
                }
            }
             */
            #endregion
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
