using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using HttpService.Abstractions;
using HttpService.Abstractions.Serializers;


namespace HttpService
{
    public class RestClient : HttpServiceClient, IRestClient
    {
        protected readonly MediaTypeWithQualityHeaderValue ContentType =
            new MediaTypeWithQualityHeaderValue("application/json");


        protected readonly ISerializer Serializer;


        public RestClient(
            HttpClient client,
            ISerializer serializer) : base(client)
        {
            Serializer = serializer;
        }


        public async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken token = default)
        {
            using (var request = CreateRequest(HttpMethod.Get, url, null, ContentType))
            {
                return await SendAsync<TResponse>(request, token).ConfigureAwait(false);
            }


        }


        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest requestData,
            CancellationToken token = default)
        {
            var content = Serializer.Serialize<TRequest>(requestData);

            using (var httpContent = Serialize(content, ContentType))
            using(var request = CreateRequest(HttpMethod.Post, url, httpContent, ContentType))
            {

                return await SendAsync<TResponse>(request, token).ConfigureAwait(false);
            }
        }


        public async Task<TResponse> DeleteAsync<TResponse>(string url, CancellationToken token = default)
        {
            using (var request = CreateRequest(HttpMethod.Delete, url, null, ContentType))
            {
                return await SendAsync<TResponse>(request, token).ConfigureAwait(false);
            }
        }


        public async Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest requestData,
            CancellationToken token = default)
        {
            var content = Serializer.Serialize<TRequest>(requestData);

            using (var httpContent = Serialize(content, ContentType))
            using(var request = CreateRequest(HttpMethod.Put, url, httpContent, ContentType))
            {
                return await SendAsync<TResponse>(request, token).ConfigureAwait(false);
            }
        }


        public virtual Task AddRequestHeadersAsync(HttpRequestHeaders headers)
        {
            return Task.CompletedTask;
        }


        public async Task<TResponse> SendAsync<TResponse>(
            HttpRequestMessage httpRequest,
            CancellationToken token = default)
        {
            await AddRequestHeadersAsync(httpRequest.Headers).ConfigureAwait(false);

            var result = await SendAsync(httpRequest, token).ConfigureAwait(false);

            if (result == null)
            {
                return default;
            }

            return (TResponse) Serializer.Deserialize(result, typeof(TResponse));
        }
    }
}
