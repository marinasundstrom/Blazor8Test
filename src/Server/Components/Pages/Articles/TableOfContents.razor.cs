using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Components.Pages.Articles;

public partial class TableOfContents
{
    private IJSObjectReference? module;

    [Inject] public IJSRuntime JS { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!firstRender) 
        { 
            module = await JS.InvokeAsync<IJSObjectReference>("import", 
                "../Components/Pages/Articles/TableOfContents.razor.js");

            await module.InvokeVoidAsync("init");
        }

        StateHasChanged();
    } 
}