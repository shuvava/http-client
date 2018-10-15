using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;


namespace HttpService.Abstractions
{
    public interface IHttpServiceClient
    {
        HttpContent Serialize(object content, MediaTypeWithQualityHeaderValue contentType = null);


        HttpRequestMessage CreateRequest(
            HttpMethod httpMethod,
            string absoluteUrl,
            HttpContent content = default,
            MediaTypeWithQualityHeaderValue acceptedContentType = default);


        Task<string> ProcessResponseAsync(HttpResponseMessage response);


        Task<string> SendAsync(
            HttpRequestMessage httpRequest,
            CancellationToken token = default);
    }
}
