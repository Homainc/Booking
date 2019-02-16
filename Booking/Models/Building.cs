using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class Building
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public Building()
        {
            Rooms = new List<Room>();
        }
    }
}
