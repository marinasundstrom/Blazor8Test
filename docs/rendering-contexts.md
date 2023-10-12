# Rendering contexts

A component can either be rendered on the server (static page or interactive), or on the client (interactive WebAssembly).

If it is an interactive component (both server and client) it can also be prerendered on the server.

The custom ``RenderingContext`` object has properties for checking where the component is being rendered, and whether it is being prerendered.

```razor
@inject RenderingContext RenderingContext

@code 
{
    protected override void OnInitialized() 
    {
        if(RenderingContext.IsServer) 
        {
            if(RenderingContext.IsPrerendering) 
            {
                
            }
        }
        else if(RenderingContext.IsClient) 
        {

        }
    }
}
```

Realistically, you don't have to check every flag.

Just be aware that the ``IsServer`` will be set to ``true`` also when ``IsPrerendering`` is set to ``true``.