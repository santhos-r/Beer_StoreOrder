using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model.Models;
namespace Beer_StoreOrder.Service.Services.Interface
{
    public interface IBarService
    {
        Task<Bar> AddBar(Bar bar);
        Task UpdateBar(long id, Bar bar);
        Task<IEnumerable<Bar>> GetBars();
        Task<ActionResult<Bar>> GetBarbyId(long id);
        public bool BarExists(long id);
    }
}