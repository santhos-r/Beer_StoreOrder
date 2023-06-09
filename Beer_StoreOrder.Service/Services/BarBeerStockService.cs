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
        public async Task PostBarBeerStock(BarBeerStock barBeerStock)
        {
            _context.BarBeerStocks.Add(barBeerStock);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Bar>> GetBarBeerStockDetailbyId(long barId)
        {
            var bar = await _context.Bars
                    .Include(e => e.BarBeerStocks)
                    .Where(b => b.Id == barId)
                    .ToListAsync();
            return bar;
        }
        public async Task<IEnumerable<Bar>> GetBarBeerStockDetail()
        {
            var bar = await _context.Bars
           .Include(e => e.BarBeerStocks)
           .ToListAsync();
            return bar;
        }
    }
}
