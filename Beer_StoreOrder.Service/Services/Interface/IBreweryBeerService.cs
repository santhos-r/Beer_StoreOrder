﻿using Beer_StoreOrder.Model.Models;
namespace Beer_StoreOrder.Service.Services.Interface
{
    public interface IBreweryBeerService
    {
        Task<Beer> AddBreweryBeer(Beer beer);
        Task<IEnumerable<Brewery>> GetBreweryBeerbyId(long breweryId);
        Task<IEnumerable<Brewery>> GetBreweryBeer();
    }
}
