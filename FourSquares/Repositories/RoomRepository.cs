using FourSquares.Data;
using FourSquares.Models;
using Microsoft.EntityFrameworkCore;

namespace FourSquares.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly FoursquareContext _context;

        public RoomRepository(FoursquareContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<Room>> GetRoomsByHotelAsync(long hotelId)
        {
            var availableRooms = await _context.Rooms
                .Where(r => r.HotelId == hotelId && r.Availability)  
                .ToListAsync();

            return availableRooms;
        }
    }
}
