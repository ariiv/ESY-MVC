namespace ESY_MVC.Models
{
    public class ProductModel
    {
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
