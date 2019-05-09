using System.IO;
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
            MediaTypeWithQualityHeaderValue acceptedContentType = default,
            AuthenticationHeaderValue authHeader = default);

        Task<string> GetResponseAsStringAsync(
            HttpRequestMessage httpRequest,
            CancellationToken token = default);

        Task<Stream> GetResponseAsStreamAsync(
            HttpRequestMessage httpRequest,
            CancellationToken token = default);
    }
}
