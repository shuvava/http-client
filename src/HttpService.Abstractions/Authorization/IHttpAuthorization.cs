using System;
using System.Threading.Tasks;


namespace HttpService.Abstractions.Authorization
{
    public interface IHttpAuthorization
    {
        string RefreshTokenUri { get; }
        DateTime ExpirationTokenTime { get; }

        Task<string> RefreshTokenAsync();
    }
}
