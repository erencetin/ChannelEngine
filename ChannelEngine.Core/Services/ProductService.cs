using ChannelEngine.Core.Interfaces;
using ChannelEngine.Core.Models;

namespace ChannelEngine.Core.Services
{
    public class ProductService : IProductService
    {
        IOrderRepository _orderRepository;
        IOfferRepository _offerRepository;
        public ProductService(IOrderRepository orderRepository, IOfferRepository offerRepository)
        {
            _orderRepository = orderRepository;
            _offerRepository = offerRepository;
        }
        public async Task<IEnumerable<Product>> GetTopFiveProducts()
        {
            var orderCollection = await _orderRepository.GetByStatus("IN_PROGRESS");
            var selectedProducts = (from order in orderCollection.Content.SelectMany(p => p.Lines)
                                    group order by new { order.MerchantProductNo, order.Description, order.Gtin } into g
                                    select new Product
                                    {
                                        Quantity = g.Count(),
                                        MerchantProductNo = g.Key.MerchantProductNo,
                                        Description = g.Key.Description,
                                        Gtin = g.Key.Gtin,
                                    }).OrderByDescending(p => p.Quantity).Take(5);

            return selectedProducts;
        }

        public async Task UpdateProductStock(string productNo, int newAmount)
        {
            if (string.IsNullOrEmpty(productNo))
                throw new ArgumentNullException("Product No should be provided!");
            if (newAmount < 0)
                throw new ArgumentOutOfRangeException("Stock amount cannot be less than zero!");

            await _offerRepository.UpdateStock(productNo, newAmount);
        }


    }
}
