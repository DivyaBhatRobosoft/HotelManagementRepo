using FourSquares.Data;
using FourSquares.Models;
using FourSquares.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourSquares.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController:ControllerBase
    {
        private readonly IRoomRepository _roomRepository;

        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        
        [HttpGet("{hotelId}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomsByHotel(long hotelId)
        {
            var rooms = await _roomRepository.GetRoomsByHotelAsync(hotelId);

            if (rooms == null || !rooms.Any())
            {
                return NotFound(); 
            }

            return Ok(rooms);  
        }

    }
}
