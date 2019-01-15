using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.DataClasses
{
    public static class MakeUnixTime
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);

        public static long GetCurrentUnixTimestampMillis()
        {
            return (long)(DateTime.Now - UnixEpoch).TotalMilliseconds;
        }

        public static long ConvertToUnixTimeUTC(DateTime dt)
        {
            dt = dt.ToUniversalTime();
            return (long)(dt - UnixEpoch).TotalSeconds;
        }

        public static long ConvertToUnixTime(DateTime dt)
        {
            return (long)(dt - UnixEpoch).TotalSeconds;
        }


        public static DateTime DateTimeFromUnixTimestampMillis(long millis)
        {
            return UnixEpoch.AddMilliseconds(millis);
        }

        public static long GetCurrentUnixTimestampSeconds()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
        }

        public static DateTime DateTimeFromUnixTimestampSeconds(long seconds)
        {
            return UnixEpoch.AddSeconds(seconds);
        }
    }
}