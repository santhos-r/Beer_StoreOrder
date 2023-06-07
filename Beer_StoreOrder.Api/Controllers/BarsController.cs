using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Beer_StoreOrder.Model;

namespace Beer_StoreOrder.Api.Controllers
{
    [Route("api/bar")]
    [ApiController]
    public class BarsController : ControllerBase
    {
        private readonly IBarService _storeService;

        public BarsController(IBarService storeService)
        {
            _storeService = storeService;
        }

        #region "POST /bar"
        // Adding Bars data in the Bars Table
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<Bar>> PostBar(Bar bar)
        {
            try
            {
                if(bar.Id==0)
                {
                    return BadRequest();
                }
                else if (BarExists(bar.Id))
                {
                    throw new ApplicationException("DuplicatedID Found");
                }
                await _storeService.PostBar(bar);
                return CreatedAtAction("PostBar", new { id = bar.Id }, bar);
            }
            catch (Exception ex)
            {               
                throw new Exception(ex.Message);                
            }
        }
        #endregion

        #region "PUT /bar/{id}"
        // Updating Bars data in the Bars Table
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutBar(long id, Bar bar)
        {
            try
            {
                if (id != bar.Id)
                {
                    throw new ApplicationException("ID Mismatching");
                }
                else if (!BarExists(id))
                {
                    throw new ApplicationException("ID Not Found");
                }
                await _storeService.PutBar(id, bar);
                return CreatedAtAction("PutBar", new { id = bar.Id }, bar);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region "GET /bar"
        // Getting Bars data in the Bars Table        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Bar>> GetBars()
        {
            try
            {
                var result = await _storeService.GetBars();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region "GET /bar/{id}"
        // Getting Bars data in the Bars Table from ID
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Bar>> GetBarbyId(long id)
        {
            try
            {
                var result = await _storeService.GetBarbyId(id);
                if (result.Value == null)
                    throw new ApplicationException("ID Not Found");
                return result;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
        #endregion

        #region "Duplicate Validation"
        private bool BarExists(long id)
        {
            var result = _storeService.BarExists(id);
            return result;
        }
        #endregion
    }
}
