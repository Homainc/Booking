using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class RoomDevice
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
