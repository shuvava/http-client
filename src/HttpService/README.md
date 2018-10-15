# HttpClient

Generic implementation REST api client functional

# Interfaces

- `IHttpServiceClient` - low level abstraction for non generic cases (for example using of MultipartFormDataContent)
- `IRestClient` high level abstraction of rest client works directly with models

# Component registration

with Microsoft Dependency Injection framework:

additional decencies

- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Http

```csharp
    var services = new ServiceCollection();
    services.AddSingleton<ISerializer, JsonRestSerializer>();
    services.AddHttpClient<IRestClient, RestClient>();

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
