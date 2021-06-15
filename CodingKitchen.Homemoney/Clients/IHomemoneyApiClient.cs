using System.Net.Http;
using System.Threading.Tasks;
using CodingKitchen.Homemoney.Models;
using Refit;

namespace CodingKitchen.Homemoney.Clients
{
    public interface IHomemoneyApiClient
    {
        [Post("/api/api2.asmx/TransactionSave")]
        Task<HttpResponseMessage> TransactionSave([Body(BodySerializationMethod.UrlEncoded)]AccountingTransaction accountingTransaction);
    }
}
