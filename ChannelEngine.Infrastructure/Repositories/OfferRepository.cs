using ChannelEngine.Core;
using ChannelEngine.Core.Interfaces;
using ChannelEngine.Core.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChannelEngine.Infrastructure.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        IHttpClientFactory _httpClientFactory;
        ApiConfiguration _apiConfiguration;
        const string ApiEndpoint = "offer";
        const string Status = "IN_PROGRESS";
        public OfferRepository(IHttpClientFactory httpClientFactory, IOptions<ApiConfiguration> apiConfiguration)
        {
            _httpClientFactory = httpClientFactory;
            _apiConfiguration = apiConfiguration.Value;
        }

        public async Task UpdateStock(string productNo, int newAmount)
        {
            var bodyMessage = JsonSerializer.Serialize(new[] {
                new
                {
                    MerchantProductNo = productNo,
                    Stock = newAmount
                }
            });
            var httpClient = _httpClientFactory.CreateClient();

            var newRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_apiConfiguration.BaseUrl}/api/v2/{ApiEndpoint}?apikey={_apiConfiguration.ApiKey}"),
                Content = new StringContent(bodyMessage, Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(newRequest, CancellationToken.None);
            response.EnsureSuccessStatusCode();
        }
    }
}
