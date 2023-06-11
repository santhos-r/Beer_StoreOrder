using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Beer_StoreOrder.Model.Models;
using Microsoft.Build.Framework;


namespace Beer_StoreOrder.Api.Controllers
{
    [Route("api/bar")]
    [ApiController]
    public class BarsController : ControllerBase
    {
        #region "Declaration"
        private readonly IBarService _barService;
        public BarsController(IBarService barService)
        {
            _barService = barService;
        }
        #endregion 

        #region "POST /bar"
        // Adding Bars data in the Bars Table
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBar(Bar bar)
        {
            if (bar.Id <= 0)
            {
                throw new ApplicationException("Bad Request");
            }
            else if (BarExists(bar.Id))
            {
                throw new ApplicationException("Same Id already exists");
            }
            var result = await _barService.AddBar(bar);
            return CreatedAtAction("AddBar", new { id = bar.Id }, result);
        }
        #endregion

        #region "PUT /bar/{id}"
        // Updating Bars data in the Bars Table
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBar(long id, Bar bar)
        {
            if (!BarExists(id))
            {
                throw new ApplicationException("ID doesnot exists");
            }
            else if (id != bar.Id)
            {
                throw new ApplicationException("ID mismatch request");
            }
            await _barService.UpdateBar(id, bar);
            return Ok(bar);
        }

        #endregion

        #region "GET /bar"
        // Getting Bars data in the Bars Table        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBars()
        {
            var result = await _barService.GetBars();
            if (result.Count() == 0)
                throw new ApplicationException("No results found");
            return Ok(result);
        }
        #endregion

        #region "GET /bar/{id}"
        // Getting Bars data in the Bars Table from ID
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBarbyId(long id)
        {
            var result = await _barService.GetBarbyId(id);
            if (result.Value == null)
                throw new ApplicationException("ID doesnot exists");
            return Ok(result.Value);
        }
        #endregion

        #region "Duplicate Validation"
        private bool BarExists(long id)
        {
            var result = _barService.BarExists(id);
            return result;
        }
        #endregion
    }
}


