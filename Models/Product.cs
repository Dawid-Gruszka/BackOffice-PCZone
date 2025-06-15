namespace BackOffice.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public string? image_url { get; set; }
        public Category category { get; set; }
 
        public Product() { }
    }
}