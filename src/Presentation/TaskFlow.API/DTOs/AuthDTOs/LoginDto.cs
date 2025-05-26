using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.DTOs.AuthDTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
