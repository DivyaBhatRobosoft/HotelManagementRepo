﻿using System;
using System.Collections.Generic;

namespace FourSquares.Models
{
    public class UserRegistrationModel
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
