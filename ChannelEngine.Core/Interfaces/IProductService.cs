using ChannelEngine.Core.Models;

namespace ChannelEngine.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetTopFiveProducts();
        Task UpdateProductStock(string productNo, int newAmount);
    }
}