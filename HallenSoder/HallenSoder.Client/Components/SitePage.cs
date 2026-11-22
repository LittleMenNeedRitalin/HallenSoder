using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HallenSoder.Client.Components;

/// <summary>Basklass för sidor: aktiverar scroll-animationerna (.reveal) efter varje rendering.</summary>
public abstract class SitePage : ComponentBase
{
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await JS.InvokeVoidAsync("hallen.observeReveal");
    }
}
