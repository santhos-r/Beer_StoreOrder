using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Api.Controllers
{
    [Route("api/beer")]
    [ApiController]
    public class BeersController : ControllerBase
    {
        #region "Declaration"
        private readonly IBeerService _storeService;
       
        public BeersController(IBeerService storeService)
        {
            _storeService = storeService;           
        }

        #endregion

        #region "POST /beer"
        // Adding Beers data in the Beer Table        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddBeer(Beer beer)
        {

            #region "Validation"
            if (beer.Id <= 0)
            {
                throw new ApplicationException("Bad Request");
            }
            else if (BeerExists(beer.Id))
            {
                throw new ApplicationException("Same ID already exists");
            }
            else if (beer.BreweryId == null || beer.BreweryId == 0)
            {
                throw new ApplicationException("BreweryID not found");
            }           
            #endregion

            var result = await _storeService.AddBeer(beer);
            return CreatedAtAction("AddBeer", new { id = beer.Id }, result);
        }
        #endregion

        #region "PUT /beer/{id}"
        // Updating Beers data in the Beer Table       
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBeer(long id, Beer beer)
        {

            #region "Validation"
            if (!BeerExists(id))
            {
                throw new ApplicationException("ID doesnot exists");
            }
            else if (id != beer.Id)
            {
                throw new ApplicationException("ID mismatch request");
            }
            else if (beer.BreweryId == null || beer.BreweryId == 0)
            {
                throw new ApplicationException("BreweryID not found");
            }
            #endregion

            await _storeService.UpdateBeer(id, beer);
            return Ok(beer);
        }
        #endregion

        #region "GET /beer?gtAlcoholByVolume=5.0&ltAlcoholByVolume=8.0"
        // Getting Beers data in the Beer Table from the From & To Alcohol range parameter
        // GET /beer?gtAlcoholByVolume=5.0&ltAlcoholByVolume=8.0 - Get all beers with optional filtering query parameters for alcohol content (gtAlcoholByVolume = greater than, ltAlcoholByVolume = less than)
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBeer([FromQuery] double gtAlcoholByVolume, [FromQuery] double ltAlcoholByVolume)
        {
            if (gtAlcoholByVolume < 0 || ltAlcoholByVolume < 0)
                throw new ApplicationException("Invalid input");
            var result = await _storeService.GetBeer(gtAlcoholByVolume, ltAlcoholByVolume);
            if (result.Count() == 0)
                throw new ApplicationException("No results found");
            return Ok(result);
        }
        #endregion

        #region "GET /beer/{id}"
        // Getting Beers data in the Beer Table from Beer Id"
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBeerbyId(long id)
        {
            var result = await _storeService.GetBeerbyId(id);
            if (result.Value == null)
                throw new ApplicationException("ID doesnot exists");
            return Ok(result.Value);
        }
        #endregion

        #region "Duplicate Validation"
        private bool BeerExists(long id)
        {
            var result = _storeService.BeerExists(id);
            return result;
        }        
        #endregion
    }
}
