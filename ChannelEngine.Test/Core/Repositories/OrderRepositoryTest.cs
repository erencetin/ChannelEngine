using ChannelEngine.Core;
using ChannelEngine.Core.Models;
using ChannelEngine.Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelEngine.Test.Core.Repositories
{
    public class OrderRepositoryTest
    {
        [Test]
        public async Task GetByStatusShouldReturnObjectWhenStatusInProgress()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var apiConfigMock = new Mock<IOptions<ApiConfiguration>>();

            apiConfigMock.Setup(x => x.Value).Returns(new ApiConfiguration
            {
                ApiKey = "1234",
                BaseUrl = "http://someapi.com"
            }) ;

            var handlerMock = new Mock<HttpMessageHandler>();
            
            var orderData = new OrderResponse();
            orderData.Content = new List<Order> {
                new Order{ Id = 1},
                new Order{ Id = 2},
                new Order{ Id = 3} 
            };
            orderData.Success = true;
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(orderData))
                }).Verifiable();
            var httpClient = new HttpClient(handlerMock.Object);
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var orderRepository = new OrderRepository(httpClientFactoryMock.Object, apiConfigMock.Object);
            var result = await orderRepository.GetByStatus("IN_PROGRESS");
            
            
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Content);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public async Task GetByStatusShouldThrowHttpRequestExceptionWhenStatusIsInvalid()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var apiConfigMock = new Mock<IOptions<ApiConfiguration>>();

            apiConfigMock.Setup(x => x.Value).Returns(new ApiConfiguration
            {
                ApiKey = "1234",
                BaseUrl = "http://someapi.com"
            });

            var handlerMock = new Mock<HttpMessageHandler>();

            
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(JsonSerializer.Serialize("test"))
                }).Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var orderRepository = new OrderRepository(httpClientFactoryMock.Object, apiConfigMock.Object);

            Assert.ThrowsAsync<HttpRequestException>(() => orderRepository.GetByStatus("INCORRECT_STATUS"));
        }
    }
}
