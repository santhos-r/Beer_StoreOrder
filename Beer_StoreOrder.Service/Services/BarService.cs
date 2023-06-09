using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Service.Services
{
    public class BarService : IBarService
    {
        //Controller wise Service creation 
        private readonly NorthwindContext _context;

        public BarService(NorthwindContext context)
        {
            _context = context;
        }
        public bool BarExists(long id)
        {
            return (_context.Bars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task PostBar(Bar bar)
        {
            _context.Bars.Add(bar);
            await _context.SaveChangesAsync();
        }
        public async Task PutBar(long id, Bar bar)
        {
            _context.Entry(bar).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Bar>> GetBars()
        {
            var bar = await _context.Bars.ToListAsync();
            return bar;
        }

        public async Task<ActionResult<Bar>> GetBarbyId(long id)
        {
            var bar = await _context.Bars.FindAsync(id);
            return bar;
        }
    }
}
