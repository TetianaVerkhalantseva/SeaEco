using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SeaEco.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var http = new HttpClient{BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)};
builder.Services.AddScoped(sp => http);

var config = await http.GetFromJsonAsync<Dictionary<string, string>>("appsettings.json");
builder.Services.AddSingleton(config);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();


