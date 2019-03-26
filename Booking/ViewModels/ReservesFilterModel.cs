using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Booking.ViewModels
{
    public class ReservesFilterModel
    {
        public string UserEmail { get; set; }
        public SelectList BuildingSelect { get; set; }
        public SelectList RoomSelect { get; set; }
        public DateTime ReserveDate { get; set; }
    }
}
