
using System.ComponentModel.DataAnnotations;

namespace RegionsUser.Models.Dto
{
    public class AddUsersDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;// Represents the username of the user
        [Required, MinLength(7)]
        public string Password { get; set; } = string.Empty; // Represents the password of the user
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty; // Represents the email of the user

    }
}
