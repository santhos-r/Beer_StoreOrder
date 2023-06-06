﻿using Microsoft.AspNetCore.Mvc;
using Northwind.Models;

namespace Beer_StoreOrder.Service.Services.Interface
{
    public interface IBarService
    {
        Task PostBar(Bar bar);
        Task PutBar(long id, Bar bar);
        Task<IEnumerable<Bar>> GetBars();
        Task<ActionResult<Bar>> GetBarbyId(long id);
        bool BarExists(long id);
    }
}