using System.Diagnostics;
using System.Threading.Tasks;
using Booking.Models;
using Booking.Units;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        private UserUnit _userUnit;

        public HomeController(ApplicationContext context)
        {
            this._userUnit = new UserUnit(context);
        }

        public IActionResult Index()
        {
            return View(_userUnit.Users.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            _userUnit.Users.Create(user);
            await _userUnit.SaveAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Explore()
        {
            ViewBag.Users = _userUnit.Users.GetAll();
            ViewBag.Roles = _userUnit.Roles.GetAll();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _userUnit.Dispose();
            base.Dispose(disposing);
        }
    }
}
