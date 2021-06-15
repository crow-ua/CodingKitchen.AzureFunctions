using System.Text.Json.Serialization;

namespace CodingKitchen.Homemoney.Models
{
    public class AccountingTransaction {
        [JsonPropertyName("Token")]
        public string Token { get; set; }
        [JsonPropertyName("AccountId")]
        public string AccountId { get; set; }
        [JsonPropertyName("type")]
        public int Type { get; set; }
        [JsonPropertyName("Total")]
        public string Total { get; set; }
        [JsonPropertyName("CurencyId")]
        public string CurrencyId { get; set; }
        [JsonPropertyName("CategoryId")]
        public string CategoryId { get; set; }
        [JsonPropertyName("Date")]
        public string Date { get; set; }
        [JsonPropertyName("Description")]
        public string Description { get; set; }
        [JsonPropertyName("TransAccountId")]
        public string TransAccountId { get; set; }
        [JsonPropertyName("TransTotal")]
        public string TransTotal { get; set; }
        [JsonPropertyName("TransCurencyId")]
        public string TransCurrencyId { get; set; }
        [JsonPropertyName("isPlan")]
        public bool IsPlan { get; set; }
        [JsonPropertyName("GUID")]
        public string GUID { get; set; }
    }
}