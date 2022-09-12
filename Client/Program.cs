global using Shared.Models;
global using Shared.Static;
global using Blazored.LocalStorage;
global using Client;
global using Client.Providers;
global using Client.Services;
global using System.Net.Http.Headers;
global using Microsoft.AspNetCore.Components.Authorization;
global using System.Net;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using Client.Static;
global using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<InMemoryDatabaseCache>();

builder.Services.AddScoped<AppAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<AppAuthenticationStateProvider>());

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
