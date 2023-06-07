using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model;

namespace Beer_StoreOrder.Service.Services.Interface
{
    public interface IBeerService
    {
        Task PostBeer(Beer beer);
        Task PutBeer(long id, Beer beer);
        Task<IEnumerable<Beer>> GetBeer([FromQuery] double gtAlcoholByVolume, [FromQuery] double ltAlcoholByVolume);
        Task<ActionResult<Beer>> GetBeerbyId(long id);
        bool BeerExists(long id);
    }
}
