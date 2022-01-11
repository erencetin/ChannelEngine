using ChannelEngine.Core.Models;

namespace ChannelEngine.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderResponse> GetByStatus(string status);
    }
}