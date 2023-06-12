using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model.Models;
namespace Beer_StoreOrder.Service.Services.Interface
{
    public interface IBreweryService
    {
        Task<Brewery> AddBrewery(Brewery brewery);
        Task UpdateBrewery(long id, Brewery brewery);
        Task<IEnumerable<Brewery>> GetBrewery();
        Task<ActionResult<Brewery>> GetBrewerybyId(long id);
        bool BreweryExists(long id);
    }
}
