using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Service.Services
{
    public class BeerService : IBeerService
    {
        //Controller wise Service creation 
        private readonly NorthwindContext _context;

        public BeerService(NorthwindContext context)
        {
            _context = context;
        }
        public bool BeerExists(long id)
        {
            return (_context.Beers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task PostBeer(Beer beer)
        {
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();
        }
        public async Task PutBeer(long id, Beer beer)
        {
            _context.Entry(beer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Beer>> GetBeer(double gtAlcoholByVolume, double ltAlcoholByVolume)
        {
            var query = _context.Beers.AsQueryable();
            if (gtAlcoholByVolume > 0)
                query = query.Where(t => t.PercentageAlcoholByVolume >= gtAlcoholByVolume);
            if (ltAlcoholByVolume > 0)
                query = query.Where(p => p.PercentageAlcoholByVolume <= ltAlcoholByVolume);
            return await query.ToListAsync();
        }

        public async Task<ActionResult<Beer>> GetBeerbyId(long id)
        {
            var beer = await _context.Beers.FindAsync(id);
            return beer;
        }
    }
}
