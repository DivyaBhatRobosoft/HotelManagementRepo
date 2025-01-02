using FourSquares.Data;
using FourSquares.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FourSquares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly FoursquareContext _context;

        public HotelController(FoursquareContext context)
        {
            _context = context;
        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<string>>> SearchHotelsByCity(string city)
        {
            try
            {
                var hotelNames = await _context.Hotels
                    .Where(h => h.City.ToLower() == city.ToLower())
                    .Select(h => h.HotelName)
                    .ToListAsync();

                if (hotelNames == null || hotelNames.Count == 0)
                {
                    return NotFound("No hotels found in this city.");
                }

                return Ok(hotelNames);
            }

            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request. Please try again later." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
            }
        }

        // Get hotel details 
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotelDetails(int id)
        {
            try {
                var hotel = await _context.Hotels
                    .Include(h => h.Reviews)
                    .FirstOrDefaultAsync(h => h.HotelId == id);

                if (hotel == null)
                    return NotFound("Hotel not found.");

                return Ok(hotel);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving hotel details. Please try again later." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<AddHotel>> AddHotel([FromBody] AddHotel hotels)
        {
            try
            {
                if (hotels == null)
                {
                    return BadRequest("Invalid hotel data.");
                }

                var userRole = User.IsInRole("Admin");
                if (!userRole)
                {
                    return Unauthorized("You do not have the required role to add a hotel.");
                }
                var hotel = new Hotel
                {
                    HotelName = hotels.HotelName,
                    Address = hotels.Address,
                    Timings = hotels.Timings,
                    Category = hotels.Category,
                    City = hotels.City
                };


                _context.Hotels.Add(hotel);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetHotelDetails), new { id = hotel.HotelId }, hotel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }


    }
}
