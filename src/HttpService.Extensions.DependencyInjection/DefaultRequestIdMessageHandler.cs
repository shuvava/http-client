using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;


namespace HttpService.Extensions.DependencyInjection
{
    /// <summary>
    /// services
    ///     .AddHttpContextAccessor()
    ///     .AddHttpClient<TargetHeadersClient>((client) => client.BaseAddress = new System.Uri("https://localhost:44324"))
    ///     .AddHttpMessageHandler<DefaultRequestIdMessageHandler>();
    /// </summary>
    public class DefaultRequestIdMessageHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DefaultRequestIdMessageHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Request-Id", _httpContextAccessor.HttpContext.TraceIdentifier);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
