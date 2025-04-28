using FourSquares.Models;
namespace FourSquares.Repositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetRoomsByHotelAsync(long hotelId);
    }
}
