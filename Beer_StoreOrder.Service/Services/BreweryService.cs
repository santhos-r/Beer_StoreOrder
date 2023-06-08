using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Service.Services.Interface;

namespace Beer_StoreOrder.Service.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly NorthwindContext _context;

        public BreweryService(NorthwindContext context)
        {
            _context = context;
        }
        bool IBreweryService.BreweryExists(long id)
        {
            return (_context.Breweries?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #region "Brewery"

        async Task IBreweryService.PostBrewery(Brewery brewery)
        {
            _context.Breweries.Add(brewery);
            await _context.SaveChangesAsync();
        }
        async Task IBreweryService.PutBrewery(long id, Brewery brewery)
        {
            _context.Entry(brewery).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        async Task<IEnumerable<Brewery>> IBreweryService.GetBrewery()
        {
            var brewery = _context.Breweries.ToList();
            return brewery;
        }

        async Task<ActionResult<Brewery>> IBreweryService.GetBrewerybyId(long id)
        {
            var brewery = await _context.Breweries.FindAsync(id);
            return brewery;
        }
        #endregion
    }
}





