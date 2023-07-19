namespace BlazorApp;

public sealed class ServerNavigationManager : RenderingContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ServerNavigationManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void NavigateTo(string uri) 
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if(httpContext is null) 
        {
            return;
        }

        var response = httpContext.Response;

        response.StatusCode = (int)System.Net.HttpStatusCode.Redirect;
        response.Headers.Location = uri;
    }
}
