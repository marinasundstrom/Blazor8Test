# Authorization

This document covers authentication and authorization.

## JSON Web Token vs Cookie

A JSON Web Token (JWT) is a container of data representing an identity and their claims. It gets validated by an external authority. It is commonly used with Web APIs.

An auth cookie is simply a cookie containing the data for authenticating. This approach has been used by traditional web apps for a long time.

### Which one should you choose?

JWTs are more suitable for when passing around credentials between multiple services, or authorizing via a third-party providers/authorities.

But for simple apps that themselves are handling both authentication and authorization, using an auth cookie is enough.

This app uses cookie authentication.

## Use cookie for authorization from client

In order to use the auth cookie when authorizing from a WebAssembly context, you need to make the ``HttpClient`` include that cookie.

We do so by creating a handler:

```csharp
using Microsoft.AspNetCore.Components.WebAssembly.Http;

public class CookieHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        return await base.SendAsync(request, cancellationToken);
    }
}
```

Then registering the handler with the ``HttpClient``:

```csharp
builder.Services
    .AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<CookieHandler>();
```

The WebAssembly part will now use the auth cookie.