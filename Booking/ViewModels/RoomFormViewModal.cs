using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.ViewModels
{
    public class RoomFormViewModel
    {
        [Required(ErrorMessage = "Необходимо ввести номер комнаты")]
        [RegularExpression(@"[0-9]+[a-zA-Z]*", ErrorMessage = "Введён неверный номер комнаты")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Необходимо ввести этаж")]
        [Range(1, 100, ErrorMessage = "Некорректный этаж (необходимо целое положительное число)")]
        public int Floor { get; set; }
    }
}
