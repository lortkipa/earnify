using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dal
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByGoogleIdAsync(string googleId);
        Task<User?> GetByEmailAsync(string email);
    }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ProjectContext _context;
        public UserRepository(ProjectContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByGoogleIdAsync(string googleId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.GoogleId == googleId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
