using Booking.Interfaces;
using Booking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Booking.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private ApplicationContext _db;

        public UserRepository(ApplicationContext context) => _db = context;

        public void Create(User item) => _db.Users.Add(item);

        public User Get(int id) => _db.Users.Find(id);

        public IEnumerable<User> GetAll() => _db.Users;

        public void Update(User item) => _db.Entry(item).State = EntityState.Modified;

        public void Delete(int id)
        {
            User user = _db.Users.Find(id);
            if (user != null)
                _db.Users.Remove(user);
        }
    }
}
