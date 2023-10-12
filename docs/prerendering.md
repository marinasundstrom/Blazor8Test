# Prerendering

Prerendering is what enables you to render an interactive component on the server - serving a static page - this is especially useful for SEO and other kinds of scraping. "Persist component state" deals with the transition from prerendered static page into interactive component on the client.

By default, Blazor prerenders interactive components on the server.

## Turn prerendering off

Prerendering can be turned off by setting it to false on the render mode.

```razor
@rendermode renderMode

@code 
{
    private static IComponentRenderMode renderMode = 
        new InteractiveWebAssemblyRenderMode(prerender: false);
}
```

Also works for ``InteractiveServerRenderMode``.

## Persist component state

Whenever you are prerendering a Razor component for either interactive Server or WebAssembly, the component gets initialized twice - once on the server, and the second time on the client. 

The prerendered component gets served as static HTML, and then the interactivity kicks in on the client, initializing the component again. This may cause some flickering. But there is a way to deal with this.

In order to eliminate that need to completely initialize the component twice - setting up stuff, or fetching data, both on client and server - you can simply persist the state of a component on the server, and automatically send it to the client.

Some other web frameworks would refer to  this feature as _"hydration"_.

It is essentially a dictionary which you add state to, and then you retrieve that state on the client.

In the following sample, prerendered state of the component gets persisted, and applies it on the client.

```csharp
@page "/prerendered-counter-2"
@rendermode RenderMode.InteractiveServer
@implements IDisposable
@inject ILogger<PrerenderedCounter2> Logger
@inject PersistentComponentState ApplicationState

<PageTitle>Prerendered Counter 2</PageTitle>

<h1>Prerendered Counter 2</h1>

<p role="status">Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount;
    private Random r = new Random();
    private PersistingComponentStateSubscription persistingSubscription;

    protected override void OnInitialized()
    {
        persistingSubscription =
            ApplicationState.RegisterOnPersisting(PersistCount);

        if (!ApplicationState.TryTakeFromJson<int>(
            "count", out var restoredCount))
        {
            currentCount = r.Next(100);
            Logger.LogInformation("currentCount set to {Count}", currentCount);
        }
        else
        {
            currentCount = restoredCount!;
            Logger.LogInformation("currentCount restored to {Count}", currentCount);
        }
    }

    private Task PersistCount()
    {
        ApplicationState.PersistAsJson("count", currentCount);

        return Task.CompletedTask;
    }

    void IDisposable.Dispose()
    {
        persistingSubscription.Dispose();
    }

    private void IncrementCount()
    {
        currentCount++;
    }
}
```