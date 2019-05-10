using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using HttpService.Abstractions;


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
            MediaTypeWithQualityHeaderValue acceptedContentType = default,
            AuthenticationHeaderValue authHeader = default)
        {
            var httpReq = new HttpRequestMessage(httpMethod, absoluteUrl);
            httpReq.Headers.Clear();

            if (acceptedContentType != null)
            {
                httpReq.Headers.Accept.Add(acceptedContentType);
            }

            if (authHeader != null)
            {
                httpReq.Headers.Authorization = authHeader;
            }

            if (HttpUtils.HasRequestBody(httpMethod) && content != null)
            {
                httpReq.Content = content;
            }

            return httpReq;
        }


        public async Task<string> GetResponseAsStringAsync(
            HttpRequestMessage httpRequest,
            CancellationToken token = default)
        {
            using (var response = await Client.SendAsync(httpRequest, token).ConfigureAwait(false))
            {
                await HttpUtils.AssertResponseAsync(response);

                return await HttpUtils.ProcessResponseAsStringAsync(response).ConfigureAwait(false);
            }
        }


        public async Task<Stream> GetResponseAsStreamAsync(
            HttpRequestMessage httpRequest,
            CancellationToken token = default)
        {
            HttpResponseMessage response = null;
            Stream stream = null;

            try
            {
                response = await Client.SendAsync(httpRequest, token).ConfigureAwait(false);
                HttpUtils.AssertResponseAsync(response).Wait(token);
                stream = await HttpUtils.ProcessResponseAsStreamAsync(response).ConfigureAwait(false);

                return new HttpStream(stream, response);
            }
            catch (Exception)
            {
                stream?.Dispose();
                response?.Dispose();

                throw;
            }
        }
    }
}
