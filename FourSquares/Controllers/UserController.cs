using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity; 
using FourSquares.Models;
using FourSquares.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;
using FourSquares.Repositories;
namespace FourSquares.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FoursquareContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JWTTokenService _jwtTokenService;
        private readonly IUserRepository _userRepository;

        public UserController(FoursquareContext context, IPasswordHasher<User> passwordHasher, JWTTokenService jwtTokenService, IUserRepository userRepository)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _userRepository = userRepository;

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

                // Define the path where the profile pictures will be stored
                string imagePath = null;

                if (model.ProfilePicture != null)
                {
                    // Define the folder to store uploaded images
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile-pictures");

                    // Ensure the folder exists, if not, create it
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    // Generate a unique file name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfilePicture.FileName);
                    var filePath = Path.Combine(uploadFolder, fileName);

                    // Save the file to the server
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {

                        await model.ProfilePicture.CopyToAsync(fileStream);  // Asynchronous copy operation
                    }

                    // Store the relative file path (to be saved in the database)
                    imagePath = "/uploads/profile-pictures/" + fileName;
                }
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = _passwordHasher.HashPassword(null, model.Password),
                    ProfilePicture = imagePath
                };
                
                await _userRepository.AddAsync(user);

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
                    ReviewText = review.ReviewText,
                    Ratings=review.Ratings

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
        [Authorize]
        [HttpGet("get-average-rating/{hotelId}")]
        public async Task<IActionResult> GetAverageRating(int hotelId)
        {
            try
            {
                var hotel = await _context.Hotels.FindAsync(hotelId);
                if (hotel == null)
                {
                    return NotFound(new { message = "Hotel not found" });
                }
                var averageRating = await _context.Reviews
                    .Where(r => r.HotelId == hotelId) 
                    .AverageAsync(r => r.Ratings);
                return Ok(new { HotelId = hotelId, AverageRating = averageRating });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while calculating the average rating. Please try again later.");
            }
        }

    }
}



