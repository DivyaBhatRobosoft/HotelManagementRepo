using System;
using System.Collections.Generic;

namespace FourSquares.Models;

public partial class Hotel
{
    public int HotelId { get; set; }

    public string HotelName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Timings { get; set; }

    public string? Category { get; set; }

    public string City { get; set; } = null!;

    public string ImageUrl { get; set; }
    public string ContactNumber { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<Room> Rooms { get; set; } 
    public ICollection<Reservation> Reservations { get; set; }
}
