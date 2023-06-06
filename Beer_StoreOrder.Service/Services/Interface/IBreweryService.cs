﻿using Microsoft.AspNetCore.Mvc;
using Northwind.Models;

namespace Beer_StoreOrder.Service.Services.Interface
{
    public interface IBreweryService
    {
        Task PostBrewery(Brewery brewery);
        Task PutBrewery(long id, Brewery brewery);
        Task<IEnumerable<Brewery>> GetBrewery();
        Task<ActionResult<Brewery>> GetBrewerybyId(long id);
        bool BreweryExists(long id);
    }
}
