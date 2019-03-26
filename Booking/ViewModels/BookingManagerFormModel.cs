using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Booking.ViewModels
{
    public class BookingManagerFormModel
    {
        [Display(Name = "Начало")]
        [Required(ErrorMessage = "Необходимо ввести время начала")]
        [DataType(DataType.Time)]
        [RegularExpression("^[0-9]{1,2}:[0-9]{2}$", ErrorMessage = "Необходимо время в формате(ЧЧ:ММ)")]
        public DateTime TimeStart { get; set; }

        [Display(Name = "Окончание")]
        [Required(ErrorMessage = "Необходимо ввести время окончания")]
        [DataType(DataType.Time)]
        [RegularExpression("^[0-9]{1,2}:[0-9]{2}$", ErrorMessage = "Необходимо время в формате(ЧЧ:ММ)")]
        public DateTime TimeEnd { get; set; }

        [Display(Name = "Пользователь")]
        [Remote(action: "CheckEmailForExist", controller: "Account", ErrorMessage = "Такого пользователя нету")]
        public string Email { get; set; }
    }
}
