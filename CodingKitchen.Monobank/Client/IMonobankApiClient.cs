using System.Collections.Generic;
using System.Threading.Tasks;
using CodingKitchen.Monobank.Models;
using Refit;

namespace CodingKitchen.Monobank.Client
{
    public interface IMonobankApiClient
    {
        [Get("/bank/currency")]
        Task<IEnumerable<CurrencyInfo>> GetExchangeRates();
        
        [Get("/personal/client-info")] 
        Task<UserInfo> GetUserInfo();
        
        [Get("/personal/statement/{account}/{from}/{to}")]
        Task<IEnumerable<StatementItem>> GetPersonalStatement(
            string account,
            long from,
            long to);
    }
}
