using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using HttpService.Abstractions;
using HttpService.Abstractions.Exceptions;


namespace HttpService
{
    public class HttpServiceClient : IHttpServiceClient
    {
        protected readonly HttpClient Client;


        public HttpServiceClient(HttpClient client)
        {
            Client = client;
        }


        public HttpContent Serialize(
            object content,
            MediaTypeWithQualityHeaderValue contentType = null)
        {
            if (content == null)
            {
                return null;
            }

            var strContent = content as string;
            var bytesContent = content as byte[];
            var streamContent = content as Stream;
            HttpContent httpContent;

            if (strContent != null)
            {
                httpContent = new StringContent(strContent);
            }
            else if (bytesContent != null)
            {
                httpContent = new ByteArrayContent(bytesContent);
            }
            else if (streamContent != null)
            {
                httpContent = new StreamContent(streamContent);
            }
            else
            {
                return null;
            }


            if (contentType != null)
            {
                httpContent.Headers.ContentType = contentType;
            }

            return httpContent;
        }


        public HttpRequestMessage CreateRequest(
            HttpMethod httpMethod,
            string absoluteUrl,
            HttpContent content = default,
            MediaTypeWithQualityHeaderValue acceptedContentType = default)
        {
            var httpReq = new HttpRequestMessage(httpMethod, absoluteUrl);
            httpReq.Headers.Clear();

            if (acceptedContentType != null)
            {
                httpReq.Headers.Accept.Add(acceptedContentType);
            }

            if (HttpUtils.HasRequestBody(httpMethod) && content != null)
            {
                httpReq.Content = content;
            }

            return httpReq;
        }


        public async Task<string> ProcessResponseAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound
                || response.StatusCode == HttpStatusCode.NoContent
                || response.StatusCode == HttpStatusCode.NotModified)
            {
                return default;
            }

            var content = default(string);

            if (
                (response.Content.Headers.ContentLength != null && response.Content.Headers.ContentLength.Value > 0)
                || response.Content.Headers.ContentType != null
                )
            {
                content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            if (response.IsSuccessStatusCode)
            {
                return content;
            }

            response.Content?.Dispose();

            throw new HttpException(response.StatusCode, response.ReasonPhrase, content);
        }


        public async Task<string> SendAsync(
            HttpRequestMessage httpRequest,
            CancellationToken token = default)
        {
            using (var response = await Client.SendAsync(httpRequest, token).ConfigureAwait(false))
            {
                return await ProcessResponseAsync(response).ConfigureAwait(false);
            }
        }
    }
}
