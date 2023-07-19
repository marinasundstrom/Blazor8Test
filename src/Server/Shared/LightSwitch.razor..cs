using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Shared;

public partial class LightSwitch : IAsyncDisposable
{
    private IJSObjectReference? module;

    [Inject] public IJSRuntime JS { get; set; }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!firstRender) 
        {
            module = await JS.InvokeAsync<IJSObjectReference>("import", 
                "../Shared/LightSwitch.razor.js");
            
            await module.InvokeVoidAsync("init");

            var mode = await GetMode();

            if(mode == "light") darkModeEnabled = false;
            if(mode == "dark") darkModeEnabled = true;
            if(mode == null) darkModeEnabled = null;
        }

        StateHasChanged();
    }

    public async ValueTask<string> GetMode() =>
        await module!.InvokeAsync<string>("getMode");

    public async ValueTask SetLightMode() =>
        await module!.InvokeVoidAsync("setLightMode");

    public async ValueTask SetDarkMode() =>
        await module!.InvokeVoidAsync("setDarkMode");

    public async ValueTask SetAutoMode() =>
        await module!.InvokeVoidAsync("setAutoMode");

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (module is not null)
        {
            await module.DisposeAsync();
        }
    }
}