using ChannelEngine.Core;
using ChannelEngine.Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelEngine.Test.Core.Repositories
{
    public class OfferRepositoryTest
    {
        [Test]
        public async Task UpdateStockShouldReturnOk()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var apiConfigMock = new Mock<IOptions<ApiConfiguration>>();

            apiConfigMock.Setup(x => x.Value).Returns(new ApiConfiguration
            {
                ApiKey = "1234",
                BaseUrl = "http://someapi.com"
            }) ;

            var handlerMock = new Mock<HttpMessageHandler>();
            

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                }).Verifiable();
            var httpClient = new HttpClient(handlerMock.Object);
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var offerRepository = new OfferRepository(httpClientFactoryMock.Object, apiConfigMock.Object);
            await offerRepository.UpdateStock("P1", 25);
            Assert.Pass();
            
        }
    }
}
