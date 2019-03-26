using System.Collections.Generic;

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

        public override string ToString()
        {
            return $"{Number}, {Floor} этаж, {Info}";
        }
    }
}
