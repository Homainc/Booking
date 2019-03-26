using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class ReserveTeamUser
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int ReserveTeamId { get; set; }
        public ReserveTeam ReserveTeam { get; set; }
    }
}
