namespace ChannelEngine.Core.Interfaces
{
    public interface IOfferRepository
    {
        Task UpdateStock(string productNo, int newAmount);
    }
}