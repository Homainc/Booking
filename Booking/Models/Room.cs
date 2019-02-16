using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int Floor { get; set; }
        public string Number { get; set; }
        public string Info { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public ICollection<RoomDevice> RoomDevices { get; set; }
        public Room()
        {
            RoomDevices = new List<RoomDevice>();
        }
    }
}
