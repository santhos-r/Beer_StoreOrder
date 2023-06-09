using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Api.Controllers
{
    [Route("api/brewery")]
    [ApiController]
    public class BreweryBeerController : ControllerBase
    {
        #region "Declaration"
        private readonly IBreweryBeerService _storeService;
        public BreweryBeerController(IBreweryBeerService storeService)
        {
            _storeService = storeService;
        }
        #endregion

        #region "POST: /brewery/beer"
        // Adding Brewery Bar data in the Bar Table
        [HttpPost("beer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Beer>> PostBreweryBeer(Beer beer)
        {
            try
            {
                await _storeService.PostBreweryBeer(beer);
                return CreatedAtAction("PostBreweryBeer", new { id = beer.Id }, beer);
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }
        #endregion

        #region "GET: /brewery/{breweryId}/beer"
        // Getting Brewery Bar data in the Bar Table from ID
        [HttpGet("{breweryId}/beer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Brewery>> GetBreweryBeerbyId(long breweryId)
        {
            try
            {
                var BreweryResult = await _storeService.GetBreweryBeerbyId(breweryId);
                if (BreweryResult.Count() == 0)
                    throw new ApplicationException("No Data Found");
                return BreweryResult;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }
        #endregion

        #region "GET /brewery/beer"
        // Getting Brewery Bar data in the Bar Table
        [HttpGet("beer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Brewery>> GetBreweryBeer()
        {
            try
            {
                var result = await _storeService.GetBreweryBeer();
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
