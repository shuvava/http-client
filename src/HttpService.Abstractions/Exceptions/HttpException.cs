using System;
using System.Net;
using System.Net.Http;


namespace HttpService.Abstractions.Exceptions
{
    [Serializable]
    public class HttpException: HttpRequestException
    {
        public HttpStatusCode StatusCode { get; }
        public string Content { get; }
        public HttpException(HttpStatusCode statusCode, string message, string content = default) :base(message)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}
