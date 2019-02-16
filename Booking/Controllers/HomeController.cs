using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Booking.Models;
using Booking.Units;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
