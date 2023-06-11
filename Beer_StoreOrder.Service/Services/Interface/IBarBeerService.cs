using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Service.Services.Interface
{
    public interface IBarBeerService
    {
        Task<BarBeer> AddBarBeer(BarBeer barBeer);
        Task<IEnumerable<Bar>> GetBarBeerbyId(long barId);
        Task<IEnumerable<Bar>> GetBarBeer();
        public bool BarBeerExists(long id);

    }
}

