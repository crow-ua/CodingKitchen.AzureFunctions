using System.Text.Json.Serialization;

namespace CodingKitchen.Monobank.Models
{
    public sealed class Webhook
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public sealed class Data
    {
        [JsonPropertyName("account")]
        public string Account { get; set; }

        [JsonPropertyName("statementItem")]
        public StatementItem StatementItem { get; set; }
    }
}
