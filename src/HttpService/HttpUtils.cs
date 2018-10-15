using System;
using System.Net.Http;


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
    }
}
