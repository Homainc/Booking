using System.Collections.Generic;

namespace Booking.Models
{
    public class ReserveTeam
    {
        public int Id { get; set; }

        public Reserve Reserve { get; set; }
        public int ReserveId { get; set; }

        public ICollection<ReserveTeamUser> ReserveTeamUser { get; set; }

        public ReserveTeam()
        {
            ReserveTeamUser = new List<ReserveTeamUser>();
        }
    }
}
