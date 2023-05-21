using System.ComponentModel.DataAnnotations;
namespace CRUDTasknWeave.Models.DTOs
{
    public class UserLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
