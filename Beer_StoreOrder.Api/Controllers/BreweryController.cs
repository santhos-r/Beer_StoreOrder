using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Api.Controllers
{
    [Route("api/brewery")]
    [ApiController]
    public class BreweryController : ControllerBase
    {
        private readonly IBreweryService _storeService;
        public BreweryController(IBreweryService storeService)
        {
            _storeService = storeService;
        }

        #region "POST: api/brewery"
        // Adding Brewery data in the Brewery Table
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Brewery>> PostBrewery(Brewery brewery)
        {
            try
            {
                if (brewery.Id == 0)
                {
                    return BadRequest();
                }
                else if(BreweryExists(brewery.Id))
                {
                    throw new ApplicationException("DuplicatedID Found");
                }
                await _storeService.PostBrewery(brewery);
                return CreatedAtAction("PostBrewery", new { id = brewery.Id }, brewery);
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }

        }
        #endregion

        #region "PUT: api/Breweries/5"
        // Updating Brewery data in the Brewery Table
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> PutBrewery(long id, Brewery brewery)
        {
            try
            {
                if (id != brewery.Id)
                {
                    throw new ApplicationException("ID Mismatching");
                }
                else if (!BreweryExists(id))
                {
                    throw new ApplicationException("ID Not Found");
                }
                await _storeService.PutBrewery(id, brewery);
                return CreatedAtAction("PutBrewery", new { id = brewery.Id }, brewery);
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }                    
        }
        #endregion

        #region "GET: GET /brewery"
        // Getting Brewery data in the Brewery Table
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Brewery>> GetBrewery()
        {          
            try
            {
                var result = await _storeService.GetBrewery();
                return result;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }
        #endregion

        #region "GET /brewery/{id}"
        // Getting Brewery data in the Brewery Table by ID
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Brewery>> GetBrewerybyId(long id)
        {
            try
            {
                var result = await _storeService.GetBrewerybyId(id);
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

        #region "Duplication validation"
        private bool BreweryExists(long id)
        {
            var result = _storeService.BreweryExists(id);
            return result;
        }
        #endregion
    }
}
