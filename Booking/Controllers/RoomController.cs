using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Room room)
        {
            if (room == null || string.IsNullOrWhiteSpace(room.Number))
                return BadRequest("Необходимо ввести номер комнаты");
            if (room.BuildingId == 0)
                return BadRequest("Необходимо выбрать здание");
            if (!(new Regex("^[0-9]+[a-zA-Z]*$").IsMatch(room.Number)))
                return BadRequest("Неккоректно введён номер комнаты (Примеры: 101а, 10, 10ба )");
            if (await _appContext.Room.AnyAsync(x => x.Number == room.Number && x.BuildingId == room.BuildingId))
                return BadRequest("Комната с таким номером уже есть");
            if (room.Floor < 1)
                return BadRequest("Некорректное значение для этажа");
            await _appContext.Room.AddAsync(room);
            await _appContext.SaveChangesAsync();
            return Ok(room);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(Room room)
        {
            if (room == null || string.IsNullOrWhiteSpace(room.Number))
                return BadRequest("Необходимо ввести номер комнаты");
            if (room.BuildingId == 0)
                return BadRequest("Необходимо выбрать здание");
            if (!(new Regex("^[0-9]+[a-zA-Z]*$").IsMatch(room.Number)))
                return BadRequest("Неккоректно введён номер комнаты (Примеры: 101а, 10, 10ба )");
            if (await _appContext.Room.AnyAsync(x => x.Number == room.Number && x.BuildingId == room.BuildingId))
                return BadRequest("Комната с таким номером уже есть");
            if (room.Floor < 1)
                return BadRequest("Некорректное значение для этажа");
            if (!await _appContext.Room.AnyAsync(x => x.Id == room.Id))
                return NotFound();
            _appContext.Entry(room).State = EntityState.Modified;
            await _appContext.SaveChangesAsync();
            return Ok(room);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
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