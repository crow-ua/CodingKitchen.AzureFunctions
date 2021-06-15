using System.Text.Json.Serialization;

namespace CodingKitchen.Monobank.Models
{
    public sealed class Account
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("balance")]
        public long Balance { get; set; }

        [JsonPropertyName("creditLimit")]
        public long CreditLimit { get; set; }

        [JsonPropertyName("currencyCode")]
        public CurrencyCode CurrencyCode { get; set; }

        [JsonPropertyName("cashbackType")]
        public string CashbackType { get; set; }
    }
}
