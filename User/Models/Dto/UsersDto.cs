using System.ComponentModel.DataAnnotations;

namespace User.Models.Dto
{
    public class UsersDto
    {
        public Guid Id { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
