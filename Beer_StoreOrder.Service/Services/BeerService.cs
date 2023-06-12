using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;
namespace Beer_StoreOrder.Service.Services
{
    public class BeerService : IBeerService
    {
        private readonly Beer_StoreOrderContext _dbContext;
        public BeerService(Beer_StoreOrderContext context)
        {
            _dbContext = context;
        }
        public bool BeerExists(long id)
        {
            return (_dbContext.Beers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<Beer> AddBeer(Beer beer)
        {
            var result = _dbContext.Beers.Add(beer);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task UpdateBeer(long id, Beer beer)
        {
            _dbContext.Entry(beer).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Beer>> GetBeer(double gtAlcoholByVolume, double ltAlcoholByVolume)
        {
            var query = _dbContext.Beers.AsQueryable();
            if (gtAlcoholByVolume >= 0)
                query = query.Where(t => t.PercentageAlcoholByVolume >= gtAlcoholByVolume);
            if (ltAlcoholByVolume > 0)
                query = query.Where(p => p.PercentageAlcoholByVolume <= ltAlcoholByVolume);
            return await query.ToListAsync();
        }
        public async Task<ActionResult<Beer>> GetBeerbyId(long id)
        {
            return await _dbContext.Beers.FindAsync(id);            
        }     
    }
}
