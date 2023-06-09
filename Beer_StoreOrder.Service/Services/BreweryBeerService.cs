using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Service.Services
{
    public class BreweryBeerService : IBreweryBeerService
    {
        private readonly NorthwindContext _context;

        public BreweryBeerService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task PostBreweryBeer(Beer beer)
        {
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Brewery>> GetBreweryBeerbyId(long breweryId)
        {
            var result = _context.Breweries
                                .Include(e => e.Beers)
                                .Where(p => p.Id == breweryId)
                                .ToList();
            return result;
        }
        public async Task<IEnumerable<Brewery>> GetBreweryBeer()
        {
            var bar = await _context.Breweries
                    .Include(e => e.Beers)
                    .ToListAsync();
            return bar;
        }

    }
}
