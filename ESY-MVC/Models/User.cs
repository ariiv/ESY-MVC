using System.ComponentModel.DataAnnotations;

namespace ESY_MVC.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
