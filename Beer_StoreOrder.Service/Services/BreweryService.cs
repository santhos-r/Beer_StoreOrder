using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Service.Services.Interface;
namespace Beer_StoreOrder.Service.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly Beer_StoreOrderContext _dbContext;

        public BreweryService(Beer_StoreOrderContext context)
        {
            _dbContext = context;
        }
        public bool BreweryExists(long id)
        {
            return (_dbContext.Breweries?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<Brewery> AddBrewery(Brewery brewery)
        {
            var result = _dbContext.Breweries.Add(brewery);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task UpdateBrewery(long id, Brewery brewery)
        {
            _dbContext.Entry(brewery).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Brewery>> GetBrewery()
        {
            return await _dbContext.Breweries.ToListAsync();
        }
        public async Task<ActionResult<Brewery>> GetBrewerybyId(long id)
        {
            return await _dbContext.Breweries.FindAsync(id);
        }
    }
}





