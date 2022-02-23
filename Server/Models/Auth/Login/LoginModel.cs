using System.ComponentModel.DataAnnotations;

namespace ClinicProject.Server.Models.Auth.Login
{
    public class LoginModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
