using NUnit.Framework;
using Moq;
using ChannelEngine.Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using ChannelEngine.Core.Models;
using System.Linq;
using ChannelEngine.Core.Services;
using System;

namespace ChannelEngine.Test.Core.Services
{
    public class ProductServiceTests
    {
        const string InProgressStatus = "IN_PROGRESS";
        [Test]
        public async Task GetTopFiveProductsShouldReturnListWithFiveOrLessItems()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var offerRepositoryMock = new Mock<IOfferRepository>();
            var orderResponse = new OrderResponse
            {
                Content = GenerateOrders().ToList()
            };
            orderRepositoryMock.Setup(x => x.GetByStatus(InProgressStatus)).Returns(Task.FromResult(orderResponse));
            var service = new ProductService(orderRepositoryMock.Object, offerRepositoryMock.Object);
            var actual = await service.GetTopFiveProducts();
            var expectedProducts = new string[] { "P1", "P2", "P3", "P4", "P5" };
            var actualProducts = actual.Select(x => x.MerchantProductNo).OrderBy(x => x).ToArray();

            Assert.AreEqual(expectedProducts, actualProducts);

        }

        [Test]
        public async Task UpdateProductStockShouldCallRepositoryMethodWhenParametersAreCorrect()
        {
            var productNo = "P1";
            var newAmount = 25;
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var offerRepositoryMock = new Mock<IOfferRepository>();
            var service = new ProductService(orderRepositoryMock.Object, offerRepositoryMock.Object);
            _= service.UpdateProductStock(productNo, newAmount);
            offerRepositoryMock.Verify(x => x.UpdateStock(productNo, newAmount), Times.Once());
        }

        [Test]
        public async Task UpdateProductStockShouldThrowArgumentNullExceptionWhenProductNoIsEmpty()
        {
            var service = new ProductService(null, null);
            Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateProductStock(string.Empty, 25));
        }

        [Test]
        public async Task UpdateProductStockShouldThrowArgumentNullExceptionWhenStockIsInvalid()
        {
            var service = new ProductService(null, null);
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateProductStock("P1", -5));
        }

        private IEnumerable<Order> GenerateOrders()
        {
            return new List<Order> {
                    new Order
                    {
                         Id = 1,
                         Status = InProgressStatus,
                         Lines = new List<Line>
                         {
                            new Line
                            {
                                 Description = "Desc1",
                                 MerchantProductNo = "P1",
                                 Gtin = "g"
                            },
                            new Line
                            {
                                 Description = "Desc2",
                                 MerchantProductNo = "P2",
                                 Gtin = "g"
                            },
                            new Line
                            {
                                 Description = "Desc2",
                                 MerchantProductNo = "P2",
                                 Gtin = "g"
                            },
                            new Line
                            {
                                 Description = "Desc3",
                                 MerchantProductNo = "P3",
                                 Gtin = "g"
                            }
                         }


                    },
                    new Order
                    {
                         Id = 2,
                         Status = InProgressStatus,
                         Lines = new List<Line>
                         {
                            new Line
                            {
                                 Description = "Desc4",
                                 MerchantProductNo = "P4",
                                 Gtin = "g"
                            },
                            new Line
                            {
                                 Description = "Desc5",
                                 MerchantProductNo = "P5",
                                 Gtin = "g"
                            },
                            new Line
                            {
                                 Description = "Desc5",
                                 MerchantProductNo = "P5",
                                 Gtin = "g"
                            }
                            ,
                            new Line
                            {
                                 Description = "Desc6",
                                 MerchantProductNo = "P6",
                                 Gtin = "g"
                            }
                            ,
                            new Line
                            {
                                 Description = "Desc7",
                                 MerchantProductNo = "P7",
                                 Gtin = "g"
                            }
                         }

                    }
                 };
        }

    }
}
