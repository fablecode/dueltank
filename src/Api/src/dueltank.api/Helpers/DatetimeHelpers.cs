using System;
using System.Globalization;

namespace dueltank.api.Helpers
{
    public static class DatetimeHelpers
    {
        public static string ToUnixEpochDate(DateTime issuedAt)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixDateTime = (issuedAt.ToUniversalTime() - epoch).TotalSeconds;
            return unixDateTime.ToString(CultureInfo.InvariantCulture);
        }
    }
}