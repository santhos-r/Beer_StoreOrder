using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;

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
        async Task IBeerService.PutBeer(long id, Beer beer)
        {
            try
            {
                _context.Entry(beer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        async Task<IEnumerable<Beer>> IBeerService.GetBeer(double gtAlcoholByVolume, double ltAlcoholByVolume)
        {
            try
            {
                var query = _context.Beers.AsQueryable();
                if (gtAlcoholByVolume > 0)
                    query = query.Where(t => t.PercentageAlcoholByVolume >= gtAlcoholByVolume);
                if (ltAlcoholByVolume > 0)
                    query = query.Where(p => p.PercentageAlcoholByVolume <= ltAlcoholByVolume);
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async Task<ActionResult<Beer>> IBeerService.GetBeerbyId(long id)
        {
            try
            {
                var beer = await _context.Beers.FindAsync(id);
                return beer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
