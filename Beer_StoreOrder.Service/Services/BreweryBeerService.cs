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

        async Task IBreweryBeerService.PostBreweryBeer(Beer beer)
        {
            try
            {
                _context.Beers.Add(beer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        async Task<IEnumerable<Brewery>> IBreweryBeerService.GetBreweryBeerbyId(long breweryId)
        {
            try
            {
                var result = _context.Breweries
                                    .Include(e => e.Beers)
                                    .Where(p => p.Id == breweryId)
                                    .ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        async Task<IEnumerable<Brewery>> IBreweryBeerService.GetBreweryBeer()
        {
            try
            {
                var bar = await _context.Breweries
                        .Include(e => e.Beers)
                        .ToListAsync();
                return bar;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
