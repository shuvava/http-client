using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using HttpService.Abstractions.Exceptions;


namespace HttpService
{
    public static class HttpUtils
    {
        public static bool HasRequestBody(HttpMethod httpVerb)
        {
            switch (httpVerb.Method.ToUpper())
            {
                case "GET":
                case "DELETE":
                case "HEAD":
                case "OPTIONS":
                    return false;
            }

            return true;
        }


        public static string JoinUrls(string baseUrl, string relativeUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                return default;
            }

            if (string.IsNullOrEmpty(relativeUrl))
            {
                return baseUrl;
            }

            var baseUri = new Uri(baseUrl);
            var relativeUri = new Uri(relativeUrl, UriKind.RelativeOrAbsolute);

            if (relativeUri.IsAbsoluteUri)
            {
                return relativeUrl;
            }

            var uri = new Uri(baseUri, relativeUri);

            return uri.ToString();
        }


        public static Task AssertResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode ||
                response.StatusCode == HttpStatusCode.NotFound
                || response.StatusCode == HttpStatusCode.NoContent
                || response.StatusCode == HttpStatusCode.NotModified)
            {
                return Task.CompletedTask;
            }

            return ProcessResponseAsStringAsync(response)
                .ContinueWith(task =>
                    {
                        var content = task.Result;
                        response.Content?.Dispose();

                        throw new HttpException(response.StatusCode, response.ReasonPhrase, content);
                    },
                    TaskContinuationOptions.ExecuteSynchronously
                );
        }


        public static bool ResponseHasContent(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound
                || response.StatusCode == HttpStatusCode.NoContent
                || response.StatusCode == HttpStatusCode.NotModified)
            {
                return false;
            }

            return response.Content.Headers.ContentType != null ||
                   response.Content.Headers.ContentLength != null &&
                   response.Content.Headers.ContentLength.Value > 0;
        }


        public static Task<string> ProcessResponseAsStringAsync(HttpResponseMessage response)
        {
            if (ResponseHasContent(response))
            {
                return response.Content.ReadAsStringAsync();
            }

            return Task.FromResult(default(string));
        }


        public static Task<Stream> ProcessResponseAsStreamAsync(HttpResponseMessage response)
        {
            if (ResponseHasContent(response))
            {
                return response.Content.ReadAsStreamAsync();
            }

            return Task.FromResult(Stream.Null);
        }
    }
}
