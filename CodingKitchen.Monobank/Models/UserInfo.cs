using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodingKitchen.Monobank.Models
{
    public sealed class UserInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("webHookUrl")]
        public string WebHookURL { get; set; }

        [JsonPropertyName("accounts")]
        public ICollection<Account> Accounts { get; set; }
    }
}
