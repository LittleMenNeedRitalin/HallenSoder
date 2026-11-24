using System.Globalization;
using HallenSoder.Client;
using HallenSoder.Client.Data;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("sv-SE");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<SupabaseClient>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<SiteContent>();

var host = builder.Build();

// Hämta sajtens texter innan första sidan ritas (max 5 sekunder – annars används standardtexterna).
try
{
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
    await host.Services.GetRequiredService<SiteContent>().LoadAsync(cts.Token);
}
catch (Exception)
{
    // Standardtexter används.
}

await host.RunAsync();
