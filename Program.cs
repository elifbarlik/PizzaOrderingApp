using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Pitzam;
using Blazored.LocalStorage;
using Pitzam.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<PizzaService>();
builder.Services.AddScoped<OrderStateService>();
builder.Services.AddBlazoredLocalStorage();
await builder.Build().RunAsync();
