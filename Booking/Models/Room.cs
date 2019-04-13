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
        public ICollection<Device> Devices { get; set; }

        public Room()
        {
            Devices = new List<Device>();
        }

        public override string ToString()
        {
            return $"{Number}, {Floor} этаж, {Info}";
        }
    }
}
