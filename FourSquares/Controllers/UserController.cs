using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity; 
using FourSquares.Models;
using FourSquares.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FourSquares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FoursquareContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JWTTokenService _jwtTokenService;

        public UserController(FoursquareContext context, IPasswordHasher<User> passwordHasher, JWTTokenService jwtTokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        // User Registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            try
            {
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    return BadRequest("Email already in use");
                }

                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = _passwordHasher.HashPassword(null, model.Password)
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok("User registered successfully");

            }

          
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        //User Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName);

                if (user == null)
                {
                    return Unauthorized("Invalid username or password");
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    return Unauthorized("Invalid username or password");
                }

                var token = _jwtTokenService.GenerateJwtToken(user);

                return Ok(new { Token = token });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, "An error occurred while processing your login request. Please try again later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [Authorize]
        [HttpPost("post-review/{hotelId}")]
        public async Task<IActionResult> PostReview([FromBody] ReviewViewModel review)
        {
            try
            {
                var hotel = await _context.Hotels.FindAsync(review.HotelId);
                if (hotel == null)
                {
                    return NotFound(new { message = "Hotel not found" });
                }

                var user = await _context.Users.FindAsync(review.UserId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                var reviews = new Review
                {

                    HotelId = review.HotelId,
                    UserId = review.UserId,
                    ReviewText = review.ReviewText

                };

                _context.Reviews.Add(reviews);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Review posted successfully" });

            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, "An error occurred while saving the review. Please try again later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }


        }
    }
}



