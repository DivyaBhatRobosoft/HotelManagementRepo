namespace FourSquares.Models
{
    public class Room
    {

        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public string RoomType { get; set; }
        public decimal Price { get; set; }
        public bool Availability { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public Hotel Hotel { get; set; } 
        public ICollection<Reservation> Reservations { get; set; }
    }
}
