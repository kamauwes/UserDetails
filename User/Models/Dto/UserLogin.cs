﻿using System.ComponentModel.DataAnnotations;

namespace User.Models.Dto
{
    public class UserLogin
    {
    
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

    }
}
