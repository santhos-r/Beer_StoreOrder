using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.Api.Controllers
{
    [Route("api/brewery")]
    [ApiController]
    public class BrewerysController : ControllerBase
    {
        #region "Declaration"
        private readonly IBreweryService _storeService;
        public BrewerysController(IBreweryService storeService)
        {
            _storeService = storeService;
        }
        #endregion

        #region "POST: /brewery"
        // Adding Brewery data in the Brewery Table
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBrewery(Brewery brewery)
        {
            if (brewery.Id <= 0)
            {
                throw new ApplicationException("Bad Request");
            }
            else if (BreweryExists(brewery.Id))
            {
                throw new ApplicationException("Same ID already exists");
            }
            var result = await _storeService.AddBrewery(brewery);
            return CreatedAtAction("AddBrewery", new { id = brewery.Id }, brewery);

        }
        #endregion

        #region "PUT: /brewery/{id}"
        // Updating Brewery data in the Brewery Table
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBrewery(long id, Brewery brewery)
        {

            if (id != brewery.Id)
            {
                throw new ApplicationException("ID mismatch request");
            }
            else if (!BreweryExists(id))
            {
                throw new ApplicationException("ID doesnot exists");
            }
            await _storeService.UpdateBrewery(id, brewery);
            return Ok(brewery);
        }
        #endregion

        #region "GET: /brewery"
        // Getting Brewery data in the Brewery Table
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBrewery()
        {
            var result = await _storeService.GetBrewery();
            if (result.Count() == 0)
                throw new ApplicationException("No results found");
            return Ok(result);
        }
        #endregion

        #region "GET /brewery/{id}"
        // Getting Brewery data in the Brewery Table by ID
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBrewerybyId(long id)
        {

            var result = await _storeService.GetBrewerybyId(id);
            if (result.Value == null)
                throw new ApplicationException("ID doesnot exists");
            return Ok(result.Value);

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
