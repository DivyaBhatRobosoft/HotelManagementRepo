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

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
