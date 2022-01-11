using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ChannelEngine.Infrastructure.Repositories;
using ChannelEngine.Core.Interfaces;
using ChannelEngine.Core;
using Microsoft.Extensions.Configuration;
using ConsoleTables;
using ChannelEngine.Core.Services;

var builder = new HostBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddHttpClient();
                   services.AddOptions();
                   services.Configure<ApiConfiguration>(hostContext.Configuration.GetSection("ApiConfiguration"));
                   services.AddTransient<IOrderRepository, OrderRepository>();
                   services.AddTransient<IOfferRepository, OfferRepository>();
                   services.AddTransient<IProductService, ProductService>();
               }).UseConsoleLifetime();

builder.ConfigureAppConfiguration(config =>
{
    config.AddJsonFile("appsettings.json");
});
var host = builder.Build();

Console.WriteLine("Top 5 Products sold!");
var productService = host.Services.GetService<IProductService>();
var result = await productService.GetTopFiveProducts();
var table = new ConsoleTable("Product No.", "Product Name", "Gtin", "Quantity");
foreach (var item in result)
{
    table.AddRow(item.MerchantProductNo, item.Description, item.Gtin, item.Quantity);
}
table.Write();
Console.WriteLine("Would you like to update stock of the product in the list? y/n");
var input = Console.ReadLine();
if (input == "y")
{
    Console.WriteLine("Please enter the product no. :");
    var productNoInput = Console.ReadLine();

    while(! result.Any(p => p.MerchantProductNo.Equals(productNoInput, StringComparison.OrdinalIgnoreCase)))
    {
        Console.WriteLine("Product no. is incorrect. Please type the correct one in the list above :");
        productNoInput = Console.ReadLine();
    }
    var realProductNumber = result.Single(p => p.MerchantProductNo.Equals(productNoInput, StringComparison.OrdinalIgnoreCase));
    await productService.UpdateProductStock(realProductNumber.MerchantProductNo, 25);
    Console.WriteLine("The stock of the selected product successfully updated.");
}
