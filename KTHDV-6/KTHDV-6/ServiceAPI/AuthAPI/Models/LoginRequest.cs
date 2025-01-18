﻿namespace AuthAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
