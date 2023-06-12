using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model.Models;
namespace Beer_StoreOrder.Api.Controllers
{
    [ApiController]
    public class BarBeersController : ControllerBase
    {
        #region "Declaration"
        private readonly IBarBeerService _barBeerService;
        public BarBeersController(IBarBeerService barBeerService)
        {
            _barBeerService = barBeerService;
        }
        #endregion

        #region "POST /bar/beer"
        // Adding Bar Beers data in the Bar Table
        [HttpPost("api/bar/beer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBarBeer(BarBeer barBeer)
        {
            if (barBeer.Id <= 0)
            {
                throw new ApplicationException("Bad Request");
            }
            else if (BarBeerExists(barBeer.Id))
            {
                throw new ApplicationException("Same ID already exists");
            }
            else if (barBeer.BeerId<=0 || barBeer.BeerId == null || barBeer.BarId <= 0 || barBeer.BarId == null)
            {
                throw new ApplicationException("ReferenceID is not valid");
            }

            var result = await _barBeerService.AddBarBeer(barBeer);
            return CreatedAtAction("AddBarBeer", new { id = barBeer.Id }, barBeer);
        }

        #endregion

        #region "GET /bar/beer" 
        // Getting Bar Beers data in the Bar Table
        [HttpGet("api/bar/beer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBarBeer()
        {
            var result = await _barBeerService.GetBarBeer();
            if (result.Count() == 0)
                throw new ApplicationException("No results found");
            return Ok(result);
        }
        #endregion

        #region "GET /bar/{barId}/beer"
        // Getting Bar Beers data in the Bar Table by ID
        [HttpGet("api/bar/{barId:int}/beer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Bar>> GetBarBeerbyId(long barId)
        {
            var StockResult = await _barBeerService.GetBarBeerbyId(barId);
            if (StockResult.Count() == 0)
                throw new ApplicationException("ID doesnot exists");
            return StockResult;
        }
        #endregion

        #region "Duplicate Validation"
        private bool BarBeerExists(long id)
        {
            var result = _barBeerService.BarBeerExists(id);
            return result;
        }
        #endregion
    }
}
