using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private UserContext _appContext;

        public DeviceController(UserContext appContext) => _appContext = appContext;

        [HttpGet]
        public async Task<IEnumerable<Device>> GetAll() => 
            await _appContext.Device.ToListAsync();

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var device = await _appContext.Device.SingleOrDefaultAsync(x => x.Id == id);
            if (device == null)
                return NotFound();
            return new ObjectResult(device);
        }

        [HttpGet("avaliable")]
        public async Task<IEnumerable<Device>> GetAvaliable() => 
            await _appContext.Device.Where(x => x.RoomId == null).ToListAsync();

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Device item)
        {
            if (item == null)
                return BadRequest();
            await _appContext.Device.AddAsync(item);
            await _appContext.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Device item)
        {
            if (item == null)
                return BadRequest();
            if (!await _appContext.Device.AnyAsync(x => x.Id == item.Id))
                return NotFound();
            _appContext.Entry(item).State = EntityState.Modified;
            await _appContext.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var device = await _appContext.Device.SingleOrDefaultAsync(x => x.Id == id);
            if (device == null)
                return NotFound();
            _appContext.Device.Remove(device);
            await _appContext.SaveChangesAsync();
            return Ok(device);
        }

        ~DeviceController() => _appContext.Dispose();
    }
}