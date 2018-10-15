namespace HttpService.Abstractions.Authorization.Models
{
    public class AuthorizationOptions
    {
        public string Url { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
