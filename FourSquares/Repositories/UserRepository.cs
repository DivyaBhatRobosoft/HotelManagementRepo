using FourSquares.Data;
using FourSquares.Models;
using Microsoft.EntityFrameworkCore;

namespace FourSquares.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FoursquareContext _context;

        public UserRepository(FoursquareContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
