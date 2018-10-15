using System;
using System.Net;
using System.Net.Http;
using System.Threading;


namespace HttpServiceUnitTests
{
    public static class HttpClientFactory
    {
        public static HttpClient CreateHttpClient(int timeout = Timeout.Infinite)
        {
            return
                new HttpClient(new HttpClientHandler
                {
                    AutomaticDecompression =
                        DecompressionMethods.Deflate | DecompressionMethods.GZip,
                    // Disable cookies for shared HttpClient
                    UseCookies = false
                })
                {
                    Timeout = TimeSpan.FromMilliseconds(timeout)
                };
        }


        public static HttpClient CreateHttpClient(string baseAddress, int timeout = Timeout.Infinite)
        {
            return
                new HttpClient(new HttpClientHandler
                {
                    AutomaticDecompression =
                        DecompressionMethods.Deflate | DecompressionMethods.GZip,
                    // Disable cookies for shared HttpClient
                    UseCookies = false
                })
                {
                    Timeout = TimeSpan.FromMilliseconds(timeout),
                    BaseAddress = new Uri(baseAddress)
                };
        }
    }
}
