using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using HttpService.Abstractions.Authorization;
using HttpService.Abstractions.Authorization.Models;
using HttpService.Abstractions.Serializers;
using HttpService.Bearer.Models;


namespace HttpService.Bearer
{
    public class BearerAuthRestClient : RestClient, IHttpAuthorization, IServiceAuthorization
    {
        private readonly AuthorizationOptions _authOptions;
        private string _accessToken;


        public BearerAuthRestClient(
            HttpClient client,
            ISerializer serializer,
            IOptions<AuthorizationOptions> authOptions) : base(client, serializer)
        {
            _authOptions = authOptions.Value;
            ExpirationTokenTime = DateTime.Now.AddSeconds(-1);
        }


        public string RefreshTokenUri => _authOptions.Url;
        public DateTime ExpirationTokenTime { get; private set; }


        public async Task<string> RefreshTokenAsync()
        {
            var authParam = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", _authOptions.ClientId),
                new KeyValuePair<string, string>("client_secret", _authOptions.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            if (!string.IsNullOrEmpty(_authOptions.Scope))
            {
                authParam.Add(new KeyValuePair<string, string>("scope", _authOptions.Scope));
            }

            var content = new FormUrlEncodedContent(authParam);
            var request = CreateRequest(HttpMethod.Post, _authOptions.Url, content, ContentType);
            var result = await GetResponseAsStringAsync(request).ConfigureAwait(false);

            var resultData = JsonConvert.DeserializeObject<BearerResponse>(result);

            ExpirationTokenTime = DateTime.Now.AddSeconds(resultData.ExpiresIn);
            _accessToken = resultData.AccessToken;

            return _accessToken;
        }


        public async Task AddAuthorizationHeaderAsync(HttpRequestHeaders headers)
        {
            if (DateTime.Now > ExpirationTokenTime)
            {
                await RefreshTokenAsync().ConfigureAwait(false);
            }

            headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }


        public override Task AddRequestHeadersAsync(HttpRequestHeaders headers)
        {
            return AddAuthorizationHeaderAsync(headers);
        }
    }
}
