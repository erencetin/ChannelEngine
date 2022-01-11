using ChannelEngine.Core;
using ChannelEngine.Core.Interfaces;
using ChannelEngine.Core.Services;
using ChannelEngine.Infrastructure.Repositories;
using ChannelEngine.WebUI;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddOptions();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var configuration = builder.Configuration.GetSection(nameof(ApiConfiguration))
                .Get<ApiConfiguration>();
builder.Services.Configure<ApiConfiguration>(c => { 
    c.BaseUrl = configuration.BaseUrl;
    c.ApiKey = configuration.ApiKey;
});

builder.Services.AddHttpClient();
builder.Services.AddTransient<IOfferRepository, OfferRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IProductService, ProductService>();
await builder.Build().RunAsync();
