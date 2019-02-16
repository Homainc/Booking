using Booking.Models;
using Booking.Repositories;
using System;
using System.Threading.Tasks;

namespace Booking.Units
{
    public class UserUnit : IDisposable
    {
        private ApplicationContext _db;
        private UserRepository _userRepository;
        private RoleRepository _roleRepository;
        private bool _disposed;

        public UserUnit(ApplicationContext context)
        {
            _db = context;
        }

        public UserRepository Users => _userRepository ?? 
            (_userRepository = new UserRepository(_db));

        public RoleRepository Roles => _roleRepository ?? 
            (_roleRepository = new RoleRepository(_db));

        public void Save() => _db.SaveChanges();

        public async Task SaveAsync() => await _db.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
