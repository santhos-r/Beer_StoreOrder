using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;
namespace Beer_StoreOrder.Service.Services
{
    public class BarService : IBarService
    {
        private readonly Beer_StoreOrderContext _dbContext;
        public BarService(Beer_StoreOrderContext context)
        {
            _dbContext = context;
        }
        public bool BarExists(long id)
        {
            return (_dbContext.Bars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<Bar> AddBar(Bar bar)
        {
           var result =  _dbContext.Bars.Add(bar);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task UpdateBar(long id, Bar bar)
        {
            _dbContext.Entry(bar).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Bar>> GetBars()
        {
            return await _dbContext.Bars.ToListAsync();            
        }
        public async Task<ActionResult<Bar>> GetBarbyId(long id)
        {
            return await _dbContext.Bars.FindAsync(id);            
        }
    }
}
