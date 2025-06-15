using System.Text.Json.Serialization;

namespace BackOffice.Models
{
    internal class Order
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("total_price")]
        public decimal TotalPrice { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("items")]
        public List<OrderItem> Items { get; set; } = new();
    }
}