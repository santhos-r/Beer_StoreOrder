using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Service.Services.Interface
{
    public interface IBeerService
    {
        Task<Beer> AddBeer(Beer beer);
        Task UpdateBeer(long id, Beer beer);
        Task<IEnumerable<Beer>> GetBeer([FromQuery] double gtAlcoholByVolume, [FromQuery] double ltAlcoholByVolume);
        Task<ActionResult<Beer>> GetBeerbyId(long id);
        bool BeerExists(long id);       
    }
}
