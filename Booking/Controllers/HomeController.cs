using Booking.Models;
using Booking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _appContext;

        public HomeController(UserContext appContext) => _appContext = appContext;

        public IActionResult Index() => View();

        [Authorize(Roles = "manager")]
        public async Task<IActionResult> Booking()
        {
            var buildings = await _appContext.Building.ToListAsync();
            var rooms = new List<Room>();
            var freeDevices = await _appContext.Device.Where(x => x.RoomId == null).ToListAsync();
            buildings.Insert(0, new Building { Id=0, Address="Не выбрано" });
            rooms.Insert(0, new Room { Id = 0, Number="Не выбрано"});
            var model = new BookingFilterViewModel
            {
                Buildings = new SelectList(buildings, "Id", "Address"),
                Rooms = new SelectList(rooms, "Id", "Number"),
                Devices = freeDevices,
                Date = DateTime.Now,
                BookingManagerForm = new BookingManagerFormModel()
            };
            return View(model);
        }
    }
}
