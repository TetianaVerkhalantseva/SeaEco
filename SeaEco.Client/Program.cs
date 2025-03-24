using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SeaEco.Client;



var builder = WebAssemblyHostBuilder.CreateDefault(args);

var http = new HttpClient{BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)};
builder.Services.AddSingleton(http);

var config = await http.GetFromJsonAsync<Dictionary<string, string>>("appsettings.json");
builder.Services.AddSingleton(config);

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


await builder.Build().RunAsync();






