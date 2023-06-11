using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model.Models;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace Beer_StoreOrder.Api.Controllers
{
    [Route("api/brewery")]
    [ApiController]
    public class BreweryBeersController : ControllerBase
    {
        #region "Declaration"
        private readonly IBreweryBeerService _storeService;
        public BreweryBeersController(IBreweryBeerService storeService)
        {
            _storeService = storeService;
        }
        #endregion

        #region "POST: /brewery/beer"
        // Adding Brewery Bar data in the Bar Table
        [HttpPost("beer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBreweryBeer(Beer beer)
        {
            if (beer.Id == 0)
            {
                throw new ApplicationException("Bad Request");
            }
            var result = await _storeService.AddBreweryBeer(beer);
            return CreatedAtAction("AddBreweryBeer", new { id = beer.Id }, result);

        }
        #endregion

        #region "GET /brewery/beer"
        // Getting Brewery Bar data in the Bar Table
        [HttpGet("beer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBreweryBeer()
        {
            var result = await _storeService.GetBreweryBeer();
            if (result.Count() == 0)
                throw new ApplicationException("No results found");
            return Ok(result);
        }
        #endregion

        #region "GET: /brewery/{breweryId}/beer"
        // Getting Brewery Bar data in the Bar Table from ID
        [HttpGet("{breweryId}/beer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBreweryBeerbyId(long breweryId)
        {
            var BreweryResult = await _storeService.GetBreweryBeerbyId(breweryId);
            if (BreweryResult.Count() == 0)
                throw new ApplicationException("ID doesnot exists");
            return Ok(BreweryResult);
        }

        #endregion
    }
}
