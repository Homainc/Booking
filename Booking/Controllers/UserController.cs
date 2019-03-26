using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<User> _users;

        public UserController(UserManager<User> userManager) => _users = userManager;

        [HttpGet("email/{pars}")]
        public async Task<IActionResult> GetByEmails(string pars)
        {
            var emails = pars.Split('&').Distinct();
            var ids = new List<string>();
            foreach(var email in emails)
            {
                var user = await _users.FindByEmailAsync(email);
                if (user == null)
                    return NotFound();
                ids.Add(user.Id);
            }
            return new JsonResult(ids);
        }
    }
}