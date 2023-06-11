using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Service.Services
{
    public class BreweryBeerService : IBreweryBeerService
    {
        private readonly Beer_StoreOrderContext _dbContext;

        public BreweryBeerService(Beer_StoreOrderContext context)
        {
            _dbContext = context;
        }
        public async Task<Beer> AddBreweryBeer(Beer beer)
        {
            var result = _dbContext.Beers.Add(beer);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<IEnumerable<Brewery>> GetBreweryBeerbyId(long breweryId)
        {
            return await _dbContext.Breweries.Include(e => e.Beers)
                                           .Where(p => p.Id == breweryId)
                                           .ToListAsync();
        }
        public async Task<IEnumerable<Brewery>> GetBreweryBeer()
        {
            var bar = await _dbContext.Breweries
                    .Include(e => e.Beers)
                    .ToListAsync();
            return bar;
        }

    }
}
