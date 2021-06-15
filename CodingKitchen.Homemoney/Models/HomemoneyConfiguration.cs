using System.Diagnostics;

namespace CodingKitchen.Homemoney.Models
{ 
    public class HomemoneyConfiguration
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Token { get; set; }
        public string AccountId { get; set; }
        public int CreditType { get; set; }
        public int DebitType { get; set; }
        public string CurencyId { get; set; }
        public string DebitCategoryId { get; set; }
        public string CreditCategoryId { get; set; }
        public string TransAccountId { get; set; }
        public string TransCurencyId { get; set; }
        public bool IsPlan { get; set; }
    }
}