using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;

namespace Beer_StoreOrder.Api.Controllers
{
    [ApiController]
    public class BarBeerStocksController : ControllerBase
    {
        private readonly IBarBeerStockService _storeService;

        public BarBeerStocksController(IBarBeerStockService storeService)
        {
            _storeService = storeService;
        }

        #region "POST /bar/beer"
        // Adding Bar Beers data in the Bar Table
        [HttpPost("api/bar/beer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BarBeerStock>> PostBarBeerStock(BarBeerStock barBeerStock)
        {
            try
            {
                await _storeService.PostBarBeerStock(barBeerStock);
                return CreatedAtAction("PostBarBeerStock", new { id = barBeerStock.Id }, barBeerStock);
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }

        #endregion

        #region "GET /bar/{barId}/beer"
        // Getting Bar Beers data in the Bar Table by ID
        [HttpGet("api/bar/{barId:int}/beer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Bar>> GetBarBeerStockDetailbyId(long barId)
        {
            try
            {
                var StockResult = await _storeService.GetBarBeerStockDetailbyId(barId);
                if (StockResult.Count() == 0)
                    throw new ApplicationException("No Data Found");
                return StockResult;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }
        #endregion

        #region "GET /bar/beer" 
        // Getting Bar Beers data in the Bar Table
        [HttpGet("api/bar/beer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Bar>> GetBarBeerStockDetail()
        {
            try
            {
                var result = await _storeService.GetBarBeerStockDetail();
                return result;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
