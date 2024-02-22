using System.ComponentModel.DataAnnotations;

namespace User.Models.Domains
{
    public class Client
    {
        public Guid Id { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;


       // public Guid RegionalId { get; set; }
        //navigation property
        //public Region Region { get; set; }
    }
}
