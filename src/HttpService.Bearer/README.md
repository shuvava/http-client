# HttpClient

Implement REST API client with Bearer authorizations

# Interfaces

- `IHttpAuthorization` authorization management
- `IServiceAuthorization` injection auth header into requests

# Component registration

with Microsoft Dependency Injection framework:

additional decencies

- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Http

```csharp
    var services = new ServiceCollection();
    services.AddSingleton<ISerializer, JsonRestSerializer>();
    services.AddHttpClient<IRestClient, BearerAuthRestClient>();

    return services.BuildServiceProvider();
````

# examples 

- GET 
    ```csharp
    var httpClient = serviceFactory.GetService<IRestClient>();
    var response = await httpClient.GetAsync<Response<User>>(url).ConfigureAwait(false);
    ```
- POST 
    ```csharp
    var httpClient = serviceFactory.GetService<IRestClient>();
    var result = await srv.PostAsync<Register, RegisterResponse>(url, model).ConfigureAwait(false);
    ```
