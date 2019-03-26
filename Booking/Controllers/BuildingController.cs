using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private UserContext _appContext;
        
        public BuildingController(UserContext appContext) =>_appContext = appContext;

        [HttpGet]
        public async Task<IEnumerable<Building>> GetAll() => 
            await _appContext.Building.Include(x => x.Rooms).OrderBy(x => x.Address).ToListAsync();

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var building = await _appContext.Building.Include(x => x.Rooms).SingleOrDefaultAsync(x => x.Id == id);
            if (building == null)
                return NotFound();
            return new ObjectResult(building);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Building item)
        {
            if (item == null)
                return BadRequest();
            await _appContext.AddAsync(item);
            await _appContext.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Building item)
        {
            if (item == null)
                return BadRequest();
            if (!await _appContext.Building.AnyAsync(x => x.Id == item.Id))
                return NotFound();
            _appContext.Entry(item).State = EntityState.Modified;
            await _appContext.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var building = await _appContext.Building.SingleOrDefaultAsync(x => x.Id == id);
            if (building == null)
                return NotFound();
            _appContext.Building.Remove(building);
            await _appContext.SaveChangesAsync();
            return Ok(building);
        }

        ~BuildingController() => _appContext.Dispose();
    }
}