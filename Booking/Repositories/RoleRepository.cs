using System.Collections.Generic;
using Booking.Interfaces;
using Booking.Models;
using System;

namespace Booking.Repositories
{
    public class RoleRepository : IRepository<Role>
    {
        private ApplicationContext _db;

        public RoleRepository(ApplicationContext context) => _db = context;

        public void Create(Role item) => throw new NotImplementedException();

        public void Delete(int id) => throw new NotImplementedException();

        public void Update(Role item) => throw new NotImplementedException();

        public Role Get(int id) => _db.Roles.Find(id);

        public IEnumerable<Role> GetAll() => _db.Roles;
    }
}
