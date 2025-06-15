namespace BackOffice.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string? image_url { get; set; }

        public Category() { }
        public Category(int id, string nombre, string description)
        {
            Id = id;
            name = nombre;
        }
    }
}
