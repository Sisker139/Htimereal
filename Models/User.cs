﻿using System.ComponentModel.DataAnnotations;

namespace Htime.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        [Required]
        public string Role { get; set; } = "Customer";
    }
}
