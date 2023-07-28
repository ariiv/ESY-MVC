namespace ESY_MVC.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TimeStamp { get; set; }
        public string Action { get; set; }
    }
}
