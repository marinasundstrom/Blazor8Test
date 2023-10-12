# Prerendering

By default, Blazor prerenders interactive components on the server.

## Turn prerendering off

Prerendering can be turned of by setting it to false.

```razor
@rendermode renderMode

@code 
{
    private static IComponentRenderMode renderMode = 
        new InteractiveWebAssemblyRenderMode(prerender: false);
}
```

Also works for ``InteractiveServerRenderMode``.