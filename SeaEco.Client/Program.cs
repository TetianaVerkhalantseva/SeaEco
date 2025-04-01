using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SeaEco.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTransient<CredentialsMessageHandler>();

// Hent config fra appsettings.json:
var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var config = await http.GetFromJsonAsync<Dictionary<string, string>>("appsettings.json");
builder.Services.AddSingleton(config);

// Registrer den navngitte HttpClient:
builder.Services.AddHttpClient("ApiClient", client =>
    {
        client.BaseAddress = new Uri(config["ApiBaseUrl"]);
    })
    .AddHttpMessageHandler<CredentialsMessageHandler>();

// Du kan eventuelt registrere en scoped HttpClient-instans slik:
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();

