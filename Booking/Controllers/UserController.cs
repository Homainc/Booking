using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Booking.Models.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<User> _users;

        public UserController(UserManager<User> userManager) => _users = userManager;

        [HttpGet]
        public async Task<IEnumerable<UserAPI>> GetAll()
        {
            var users = await _users.Users.ToListAsync();
            return users.Select(x => new UserAPI(x.Id, _users.GetRolesAsync(x).Result.First(), x.Name, x.Surname, x.Email, x.EmailConfirmed));
        }

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

        [HttpPut]
        public async Task<IActionResult> Update(UserAPI user)
        {
            var dbUser = await _users.FindByIdAsync(user.Id);
            if (dbUser == null)
                return NotFound();
            var dbUserRole = (await _users.GetRolesAsync(dbUser)).First();
            if (dbUserRole != user.RoleName)
            {
                var addToRoleResult = await _users.AddToRoleAsync(dbUser, user.RoleName);
                var removeFromRoleResult = await _users.RemoveFromRoleAsync(dbUser, dbUserRole);
                if (!addToRoleResult.Succeeded)
                    return BadRequest(addToRoleResult.Errors);
                if (!removeFromRoleResult.Succeeded)
                    return BadRequest(removeFromRoleResult.Errors);
            }
            if (dbUser.Email != user.Email)
            {
                var token = await _users.GenerateChangeEmailTokenAsync(dbUser, user.Email);
                var changeEmailResult = await _users.ChangeEmailAsync(dbUser, user.Email, token);
                if (!changeEmailResult.Succeeded)
                    return BadRequest(changeEmailResult.Errors);
            }
            dbUser.Name = user.Name;
            dbUser.Surname = user.Surname;
            var result = await _users.UpdateAsync(dbUser);
            if (result.Succeeded)
                return Ok(user);
            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _users.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var result = await _users.DeleteAsync(user);
            if(result.Succeeded)
                return Ok(user);
            return BadRequest(result.Errors);
        }
    }
}