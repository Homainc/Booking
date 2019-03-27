using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Booking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Booking.Controllers
{
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
    }
}