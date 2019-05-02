using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Booking.Models;
using Booking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Booking.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private UserContext _appContext;

        public AdminController(UserContext context) => _appContext = context; 

        public async Task<IActionResult> Reserves()
        {
            var buildings = await _appContext.Building.ToListAsync();
            var rooms = new List<Room>();
            rooms.Insert(0, new Room { Id = 0, Number = "Не выбрано" });
            buildings.Insert(0, new Building { Id = 0, Address = "Не выбрано" });
            var model = new ReservesFilterModel
            {
                BuildingSelect = new SelectList(buildings, "Id", "Address"),
                RoomSelect = new SelectList(rooms, "Id", "Number")
            };
            return View(model);
        }

        public IActionResult Buildings() => View();

        public async Task<IActionResult> Rooms()
        {
            var buildings = await _appContext.Building.ToListAsync();
            buildings.Insert(0, new Building { Id = 0, Address = "Не выбрано" });
            ViewBag.BuildingsList = new SelectList(buildings, "Id", "Address");
            return View();
        }

        public IActionResult Devices() => View();

        public IActionResult Users() => View();

        public async Task<String> ReservesXML()
        {
            var reserves = await _appContext.Reserve
                .Include(x => x.User)
                .Include(x => x.Team).ThenInclude(x => x.ReserveTeamUser)
                .Include(x => x.Room).ThenInclude(x => x.Building)
                .ToListAsync();
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            settings.PreserveReferencesHandling = PreserveReferencesHandling.All;
            var json = JsonConvert.SerializeObject(reserves, Formatting.Indented, settings);
            var xml = JsonConvert.DeserializeXmlNode("{\"reserve\":"+json+"}", "root");
            return xml.OuterXml;
        }
    }
}