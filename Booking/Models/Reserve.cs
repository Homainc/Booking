using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class Reserve
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public double Hours { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
        public ReserveTeam Team { get; set; }
    }
}
