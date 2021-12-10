using System;

namespace RM_API_SDK_CSHARP.Model.Transaction
{
    public class Expiry
    {
        public ExpiryType Type { get; set; }
        public int Day { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
