using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model;

namespace Beer_StoreOrder.Api.Controllers
{
    [Route("api/beer")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        private readonly IBeerService _storeService;

        public BeerController(IBeerService storeService)
        {
            _storeService = storeService;
        }

        #region "POST /beer"
        // Adding Beers data in the Beer Table        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Beer>> PostBeer(Beer beer)
        {
            try
            {
                if (beer.Id == 0)
                {
                    return BadRequest();
                }
                else if (BeerExists(beer.Id))
                {
                    throw new ApplicationException("DuplicatedID Found");
                }
                await _storeService.PostBeer(beer);
                return CreatedAtAction("PostBeer", new { id = beer.Id }, beer);
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }
        #endregion

        #region "PUT /beer/{id}"
        // Updating Beers data in the Beer Table       
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutBeer(long id, Beer beer)
        {
            try
            {
                if (id != beer.Id)
                {
                    throw new ApplicationException("ID Mismatching");
                }
                else if (!BeerExists(id))
                {
                    throw new ApplicationException("ID Not Found");
                }
                await _storeService.PutBeer(id, beer);
                return CreatedAtAction("PutBeer", new { id = beer.Id }, beer);
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }            
        }
        #endregion

        #region "GET /beer?gtAlcoholByVolume=5.0&ltAlcoholByVolume=8.0"
        // Getting Beers data in the Beer Table from the From & To Alcohol range parameter
        // GET /beer?gtAlcoholByVolume=5.0&ltAlcoholByVolume=8.0 - Get all beers with optional filtering query parameters for alcohol content (gtAlcoholByVolume = greater than, ltAlcoholByVolume = less than)
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Beer>> GetBeer([FromQuery] double gtAlcoholByVolume, [FromQuery] double ltAlcoholByVolume)
        {
            try
            {
                var result = await _storeService.GetBeer(gtAlcoholByVolume, ltAlcoholByVolume);
                if (result.Count() == 0)
                    throw new ApplicationException("No Data Found");
                return result;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }
        #endregion

        #region "GET /beer/{id}"
        // Getting Beers data in the Beer Table from Beer Id"
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Beer>> GetBeerbyId(long id)
        {
            try
            {
                var result = await _storeService.GetBeerbyId(id);
                if (result.Value == null)
                    throw new ApplicationException("ID Not Found");
                return result;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
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
