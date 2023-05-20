using System.ComponentModel.DataAnnotations;
namespace CRUDTasknWeave.Models.DTOs
{
    public class UserLogin
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
