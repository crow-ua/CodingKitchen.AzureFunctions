using System.Text.Json.Serialization;

namespace CodingKitchen.Monobank.Models
{
    public sealed class Error
    {
        [JsonPropertyName("errorDescription")]
        public string Description { get; set; }
    }
}
