﻿using Beer_StoreOrder.Service.Services.Interface;
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
        bool IBarService.BarExists(long id)
        {
            return (_context.Bars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        async Task IBarService.PostBar(Bar bar)
        {
            _context.Bars.Add(bar);
            await _context.SaveChangesAsync();
        }
        async Task IBarService.PutBar(long id, Bar bar)
        {
            _context.Entry(bar).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        async Task<IEnumerable<Bar>> IBarService.GetBars()
        {
            var bar = await _context.Bars.ToListAsync();
            return bar;
        }

        async Task<ActionResult<Bar>> IBarService.GetBarbyId(long id)
        {
            var bar = await _context.Bars.FindAsync(id);
            return bar;
        }
    }
}
