namespace BlazorApp;

public sealed class ServerNavigationManager
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

        httpContext.Response.Redirect(uri);
    }
}
