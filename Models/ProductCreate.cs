namespace BackOffice.Models;
using System.Text.Json.Serialization;

public class ProductCreate
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("price")]
    public int Price { get; set; }

    [JsonPropertyName("category_id")]
    public int CategoryId { get; set; }
}