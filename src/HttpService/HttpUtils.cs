using System;
using System.Net;
using System.Net.Http;

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

        public static void AssertResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode ||
                response.StatusCode == HttpStatusCode.NotFound
                || response.StatusCode == HttpStatusCode.NoContent
                || response.StatusCode == HttpStatusCode.NotModified)
            {
                return;
            }

            response.Content?.Dispose();

            throw new HttpException(response.StatusCode, response.ReasonPhrase);
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
                   (
                       response.Content.Headers.ContentLength != null &&
                       response.Content.Headers.ContentLength.Value > 0
                   );
        }
    }
}
