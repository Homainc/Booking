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
    public class RoomController : ControllerBase
    {
        private UserContext _appContext;

        public RoomController(UserContext appContext) => _appContext = appContext;

        [HttpGet]
        public async Task<IEnumerable<Room>> GetAll() => 
            await _appContext.Room.Include(x => x.Building).OrderBy(x => x.Building.Address).ToListAsync();

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var room = await _appContext.Room
                .Include(x => x.Building)
                .Include(x => x.Devices)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (room == null)
                return NotFound();
            return new ObjectResult(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Room room)
        {
            if (room == null)
                return BadRequest();
            await _appContext.Room.AddAsync(room);
            await _appContext.SaveChangesAsync();
            return Ok(room);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Room room)
        {
            if (room == null)
                return BadRequest();
            if (!await _appContext.Room.AnyAsync(x => x.Id == room.Id))
                return NotFound();
            _appContext.Entry(room).State = EntityState.Modified;
            await _appContext.SaveChangesAsync();
            return Ok(room);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _appContext.Room.FirstOrDefaultAsync(x => x.Id == id);
            if (room == null)
                return NotFound();
            _appContext.Room.Remove(room);
            await _appContext.SaveChangesAsync();
            return Ok(room);
        }

        ~RoomController() => _appContext.Dispose();
    }
}