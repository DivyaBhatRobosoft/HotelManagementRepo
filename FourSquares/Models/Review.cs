using System;
using System.Collections.Generic;

namespace FourSquares.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int UserId { get; set; }

    public int HotelId { get; set; }

    public string? ReviewText { get; set; }

    public int Ratings { get; set; }    

    public DateTime? DateAdded { get; set; }

    public virtual Hotel Hotel { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
