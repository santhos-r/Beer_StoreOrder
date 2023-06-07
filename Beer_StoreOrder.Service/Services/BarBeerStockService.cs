using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Service.Services
{
    public class BarBeerStockService : IBarBeerStockService
    {
        //Controller wise Service creation 

        private readonly NorthwindContext _context;
        public BarBeerStockService(NorthwindContext context)
        {
            _context = context;
        }
        async Task IBarBeerStockService.PostBarBeerStock(BarBeerStock barBeerStock)
        {
            try
            {
                _context.BarBeerStocks.Add(barBeerStock);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        async Task<IEnumerable<Bar>> IBarBeerStockService.GetBarBeerStockDetailbyId(long barId)
        {
            try
            {
                var bar = await _context.Bars
                        .Include(e => e.BarBeerStocks)
                        .Where(b => b.Id == barId)
                        .ToListAsync();
                return bar;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        async Task<IEnumerable<Bar>> IBarBeerStockService.GetBarBeerStockDetail()
        {
            try
            {
                var bar = await _context.Bars
               .Include(e => e.BarBeerStocks)
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
