using FourSquares.Models;

namespace FourSquares.Repositories
{
    public interface IHotelRepository
    {
        Task<List<string>> GetHotelsByCityAsync(string city);
        Task<Hotel> GetHotelByIdAsync(long hotelId);
        Task CreateHotelAsync(Hotel hotel);
    }
}
