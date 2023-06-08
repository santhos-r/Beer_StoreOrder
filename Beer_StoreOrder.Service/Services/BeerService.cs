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
        bool IBeerService.BeerExists(long id)
        {
            return (_context.Beers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        async Task IBeerService.PostBeer(Beer beer)
        {
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();
        }
        async Task IBeerService.PutBeer(long id, Beer beer)
        {
            _context.Entry(beer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        async Task<IEnumerable<Beer>> IBeerService.GetBeer(double gtAlcoholByVolume, double ltAlcoholByVolume)
        {
            var query = _context.Beers.AsQueryable();
            if (gtAlcoholByVolume > 0)
                query = query.Where(t => t.PercentageAlcoholByVolume >= gtAlcoholByVolume);
            if (ltAlcoholByVolume > 0)
                query = query.Where(p => p.PercentageAlcoholByVolume <= ltAlcoholByVolume);
            return await query.ToListAsync();
        }

        async Task<ActionResult<Beer>> IBeerService.GetBeerbyId(long id)
        {
            var beer = await _context.Beers.FindAsync(id);
            return beer;
        }
    }
}
