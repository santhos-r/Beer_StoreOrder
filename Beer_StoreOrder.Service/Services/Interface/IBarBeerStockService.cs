using Northwind.Models;

namespace Beer_StoreOrder.Service.Services.Interface
{
    public interface IBarBeerStockService
    {
        Task PostBarBeerStock(BarBeerStock barBeerStock);
        Task<IEnumerable<Bar>> GetBarBeerStockDetailbyId(long barId);
        Task<IEnumerable<Bar>> GetBarBeerStockDetail();

    }
}

