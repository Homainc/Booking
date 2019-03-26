using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Models.API
{
    public class ReserveAPI
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RoomId { get; set; }
        public ISet<string> Participants { get; set; }
        public ISet<int> Devices { get; set; }

        public ReserveAPI()
        {
            Participants = new HashSet<string>();
            Devices = new HashSet<int>();
        }
    }
}
