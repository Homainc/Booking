﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Booking.Models.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private UserContext _appContext;
        private UserManager<User> _userManager;
        private ILogger _log;

        public ReserveController(UserContext appContext, ILogger<ReserveController> log, UserManager<User> userManager)
        {
            _log = log;
            _appContext = appContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<Reserve>> GetAll() =>
            await _appContext.Reserve
                .Include(x => x.User)
                .Include(x => x.Room).ThenInclude(x => x.Building)
                .Include(x => x.Team).ThenInclude(x => x.ReserveTeamUser)
                .OrderBy(x => x.DateTime)
                .ToListAsync();

        [HttpGet("user")]
        public async Task<IActionResult> GetByUser()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
                return NotFound();
            var userReserves = await _appContext.Reserve
                .Include(x => x.User)
                .Include(x => x.Room).ThenInclude(x => x.Building)
                .Include(x => x.Team).ThenInclude(x => x.ReserveTeamUser)
                .Where(x => x.Team.ReserveTeamUser.Any(y => y.UserId == user.Id))
                .OrderBy(x => x.DateTime)
                .ToListAsync();
            return new JsonResult(userReserves);
        }

        [HttpGet("manager")]
        public async Task<IActionResult> GetByManager()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
                return NotFound();
            var userReserves = await _appContext.Reserve
                .Include(x => x.Room).ThenInclude(x => x.Building)
                .Include(x => x.Team).ThenInclude(x => x.ReserveTeamUser)
                .Include(x => x.User)
                .Where(x => x.UserId == user.Id)
                .OrderBy(x => x.DateTime)
                .ToListAsync();
            return new JsonResult(userReserves);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var reserve = await _appContext.Reserve.SingleOrDefaultAsync(x => x.Id == id);
            if (reserve == null)
                return NotFound();
            return new ObjectResult(reserve);
        }

        [HttpGet("{id:int}/{date}")]
        public async Task<IActionResult> GetByRoomAndDate(int id, string date)
        {
            DateTime bookDate;
            if(!DateTime.TryParse(date, out bookDate))
                return BadRequest();
            var reserve = await _appContext.Reserve
                .Include(x => x.User)
                .Where(x => x.RoomId == id && x.DateTime.Date.CompareTo(bookDate.Date) == 0)
                .OrderBy(x => x.DateTime)
                .ToListAsync();
            return new JsonResult(reserve);
        }

        [HttpGet("{buildingId:int}-{roomId:int}/{userEmail}/{eventDate}")]
        public async Task<IActionResult> GetAllWithFilter([FromRoute]int buildingId, int roomId, string eventDate, string userEmail)
        {
            IQueryable<Reserve> reserves = _appContext.Reserve
                .Include(x => x.User)
                .Include(x => x.Room).ThenInclude(x => x.Building)
                .Include(x => x.Team).ThenInclude(x => x.ReserveTeamUser)
                .OrderBy(x => x.DateTime);
            if (buildingId != 0)
                reserves = reserves.Where(x => x.Room.BuildingId == buildingId);
            if (roomId != 0)
                reserves = reserves.Where(x => x.RoomId == roomId);
            if (userEmail != "null")
                reserves = reserves.Where(x => x.User.Email.ToLower().Contains(userEmail.ToLower()));
            if (eventDate != "null")
            {
                var eDate = DateTime.Parse(eventDate);
                reserves = reserves.Where(x => x.DateTime.Date == eDate.Date);
            }
            return new JsonResult(await reserves.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReserveAPI item)
        {
            if (item == null)
                return BadRequest();
            if (item.Participants.Contains(User.Identity.Name))
                return BadRequest("Нельзя добавлять себя в участников!");
            var participants = await _appContext.Users.Where(x => item.Participants.Contains(x.Email)).ToListAsync();
            double dHours = item.EndDate.Hour - item.StartDate.Hour;
            dHours += (item.EndDate.Minute - item.StartDate.Minute) / 60.0;
            var user = await _appContext.Users.SingleOrDefaultAsync(x => x.Email == User.Identity.Name);
            var reserve = new Reserve
            {
                DateTime = item.StartDate,
                Hours = dHours,
                RoomId = item.RoomId,
                UserId = user.Id                
            };
            var checkReserves = _appContext.Reserve.Where(x => x.DateTime.Date == item.StartDate.Date 
                && ((x.DateTime > item.StartDate && x.DateTime < item.EndDate)
                || (x.DateTime.AddHours(x.Hours) > item.StartDate && x.DateTime.AddHours(x.Hours) < item.EndDate)));
            if (checkReserves.Count() != 0)
                return BadRequest("Заказ пересекается с другим заказом по времени!");
            var devList = await _appContext.Device.Where(x => item.Devices.Contains(x.Id)).ToListAsync();
            devList.ForEach(x => x.RoomId = item.RoomId);
            await _appContext.Reserve.AddAsync(reserve);
            var reserveTeam = new ReserveTeam{ReserveId = reserve.Id};
            await _appContext.ReserveTeam.AddAsync(reserveTeam);
            participants.ForEach(x => x.ReserveTeamUser.Add(new ReserveTeamUser { ReserveTeamId = reserveTeam.Id, UserId = x.Id}));
            await _appContext.SaveChangesAsync();
            return Ok(reserve);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Reserve item)
        {
            if (item == null)
                return BadRequest();
            if (!await _appContext.Reserve.AnyAsync(x => x.Id == item.Id))
                return NotFound();
            _appContext.Entry(item).State = EntityState.Modified;
            await _appContext.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPut("decline/{id:int}")]
        public async Task<IActionResult> DeclineReserve(int id)
        {
            var reserve = await _appContext.Reserve
                .Include(x => x.Team).ThenInclude(x => x.ReserveTeamUser)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (reserve == null)
                return NotFound();
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if(reserve.Team.ReserveTeamUser.Any(x => x.UserId == user.Id))
            {
                var deleted = reserve.Team.ReserveTeamUser.Single(x => x.UserId == user.Id);
                reserve.Team.ReserveTeamUser.Remove(deleted);
                _appContext.Entry(reserve).State = EntityState.Modified;
                await _appContext.SaveChangesAsync();
                return Ok(reserve);
            }
            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reserve = await _appContext.Reserve.SingleOrDefaultAsync(x => x.Id == id);
            if (reserve == null)    
                return NotFound();
            _appContext.Reserve.Remove(reserve);
            await _appContext.SaveChangesAsync();
            return Ok(reserve);
        }

        ~ReserveController() => _appContext.Dispose();
    }
}