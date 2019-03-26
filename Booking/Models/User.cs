using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Booking.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<Reserve> Reserves { get; set; }
        public ICollection<ReserveTeamUser> ReserveTeamUser { get; set; }

        public User() : base()
        {
            Reserves = new List<Reserve>();
            ReserveTeamUser = new List<ReserveTeamUser>();
        }
    }
}
