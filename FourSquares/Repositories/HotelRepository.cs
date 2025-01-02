using FourSquares.Data;
using FourSquares.Models;
using Microsoft.EntityFrameworkCore;

namespace FourSquares.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly FoursquareContext _context;

        public HotelRepository(FoursquareContext context)
        {

            _context = context;
            
        }
        public async Task CreateHotelAsync(Hotel hotel)
        {

            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task<Hotel> GetHotelByIdAsync(long hotelId)
        {
           return  await _context.Hotels
                    .Include(h => h.Reviews)
                    .FirstOrDefaultAsync(h => h.HotelId == hotelId);
        }

        public async Task<List<string>> GetHotelsByCityAsync(string city)
        {
            return await  _context.Hotels
                    .Where(h => h.City.ToLower() == city.ToLower())
                    .Select(h => h.HotelName)
                    .ToListAsync();
        }
    }
}
