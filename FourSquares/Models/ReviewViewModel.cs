using System;
using System.Collections.Generic;


namespace FourSquares.Models
{
    public class ReviewViewModel
    {
        public int UserId { get; set; }

        public int HotelId { get; set; }

        public string? ReviewText { get; set; }

        public DateTime? DateAdded { get; set; }
    }
}
