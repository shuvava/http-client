using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;


namespace HttpService.Abstractions
{
    public interface IRestClient: IHttpServiceClient
    {
        Task<TResponse> GetAsync<TResponse>(string url, CancellationToken token = default);
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest requestData, CancellationToken token = default);
        Task<TResponse> DeleteAsync<TResponse>(string url, CancellationToken token = default);
        Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest requestData, CancellationToken token = default);
        Task<TResponse> SendAsync<TResponse>(HttpRequestMessage httpRequest, CancellationToken token = default);
        Task AddRequestHeadersAsync(HttpRequestHeaders headers);
    }
}
