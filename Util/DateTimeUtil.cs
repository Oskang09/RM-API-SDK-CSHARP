using System;
using System.Linq;

namespace RM_API_SDK_CSHARP.Util
{
    public static class DateTimeUtil
    {

        public static Int64 UnixSecond(this DateTime dateTime)
        {
            return (Int64)dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}
