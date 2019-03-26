using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoomDevice> RoomDevices { get; set; }

        public Device()
        {
            RoomDevices = new List<RoomDevice>();
        }

        public override string ToString() => Name;
    }
}
