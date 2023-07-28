using System.ComponentModel.DataAnnotations;

namespace ESY_MVC.Models
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int UnitCount { get; set; }
        [Required]
        public double PricePerUnit { get; set; }
    }
}
