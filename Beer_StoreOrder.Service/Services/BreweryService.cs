using Northwind.Data;
using Northwind.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
            try
            {
                _context.Breweries.Add(brewery);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        async Task IBreweryService.PutBrewery(long id, Northwind.Models.Brewery brewery)
        {
            try
            {
                _context.Entry(brewery).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async Task<IEnumerable<Brewery>> IBreweryService.GetBrewery()
        {
            try
            {
                var brewery = _context.Breweries.ToList();
                return brewery;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async Task<ActionResult<Brewery>> IBreweryService.GetBrewerybyId(long id)
        {
            try
            {
                var brewery = await _context.Breweries.FindAsync(id);
                return brewery;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}





