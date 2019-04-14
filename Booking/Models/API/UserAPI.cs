using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Models.API
{
    public class UserAPI
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }

        public UserAPI() {}

        public UserAPI(string id, string roleName, string name, string surname, string email, bool isEmailConfirmed)
        {
            this.RoleName = roleName;
            this.Name = name;
            this.Surname = surname;
            this.Email = email;
            this.IsEmailConfirmed = isEmailConfirmed;
            this.Id = id;
        }
    }
}
