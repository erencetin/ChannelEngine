using ChannelEngine.Core;
using ChannelEngine.Core.Interfaces;
using ChannelEngine.Core.Models;
using ChannelEngine.Core.Specifications;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ChannelEngine.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        IHttpClientFactory _httpClientFactory;
        ApiConfiguration _apiConfiguration;
        const string ApiEndpoint = "orders";
        public OrderRepository(IHttpClientFactory httpClientFactory, IOptions<ApiConfiguration> apiConfiguration)
        {
            _httpClientFactory = httpClientFactory;
            _apiConfiguration = apiConfiguration.Value;
        }
        public async Task<OrderResponse> GetByStatus(string status)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var newRequest = new HttpRequestMessage(HttpMethod.Get,
                $"{_apiConfiguration.BaseUrl}/api/v2/{ApiEndpoint}?apikey={_apiConfiguration.ApiKey}&statuses={status}");
            var response = await httpClient.SendAsync(newRequest, CancellationToken.None);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderResponse>();
        }
    }
}
