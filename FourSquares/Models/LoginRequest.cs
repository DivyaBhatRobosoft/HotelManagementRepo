using System;
using System.Collections.Generic;

namespace FourSquares.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
