using System.ComponentModel.DataAnnotations;

namespace FourSquares.Models
{
    public class Reservation
    {
        [Key]
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string BookingStatus { get; set; }

        public User User { get; set; } 
        public Hotel Hotel { get; set; } 
        public Room Room { get; set; } 
      
    }
}
