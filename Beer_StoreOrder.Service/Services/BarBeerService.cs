using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Service.Services
{
    public class BarBeerService : IBarBeerService
    {
        //Controller wise Service creation 

        private readonly Beer_StoreOrderContext _dbContext;
        public BarBeerService(Beer_StoreOrderContext context)
        {
            _dbContext = context;
        }
        public bool BarBeerExists(long id)
        {
            return (_dbContext.BarBeers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<BarBeer> AddBarBeer(BarBeer barBeer)
        {
            var result = _dbContext.BarBeers.Add(barBeer);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<IEnumerable<Bar>> GetBarBeerbyId(long barId)
        {
            return await _dbContext.Bars.Include(e => e.BarBeers)
                                      .Where(b => b.Id == barId)
                                      .ToListAsync();
        }
        public async Task<IEnumerable<Bar>> GetBarBeer()
        {
            return await _dbContext.Bars.Include(e => e.BarBeers).ToListAsync();
        }
    }
}
