using Booking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Booking.ViewModels
{
    public class BookingFilterViewModel
    {
        [Display(Name = "Здание")]
        public SelectList Buildings { get; set; }

        [Display(Name = "Помещение")]
        public SelectList Rooms { get; set; }

        [Display(Name = "Оборудование")]
        public IEnumerable<Device> Devices { get; set; } 

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        public BookingManagerFormModel BookingManagerForm { get; set; }
    }
}
