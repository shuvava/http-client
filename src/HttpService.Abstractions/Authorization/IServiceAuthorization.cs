using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace HttpService.Abstractions.Authorization
{
    public interface IServiceAuthorization
    {
        Task AddAuthorizationHeaderAsync(HttpRequestHeaders headers);
    }
}
