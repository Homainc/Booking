using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class ReserveTeam
    {
        [Key]
        [ForeignKey("Reserve")]
        public int Id { get; set; }
        public ICollection<User> Users { get; set; }
        public Reserve Reserve { get; set; }
        public ReserveTeam()
        {
            Users = new List<User>();
        }
    }
}
