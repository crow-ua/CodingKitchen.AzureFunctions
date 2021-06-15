using System;

namespace CodingKitchen.AzureFunctions.Extensions
{
    internal static class DateTimeExtensions
    {
        internal static long ToUnixTime(this DateTime date)
        {
            return new DateTimeOffset(date).ToUnixTimeSeconds();
        }
        
        internal static DateTime ToDateTime(this long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime.ToLocalTime();
        }
    }
}
