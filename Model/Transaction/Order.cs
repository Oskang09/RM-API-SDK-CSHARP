using System;

namespace RM_API_SDK_CSHARP.Model.Transaction
{
    public class Order
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public Int64 Amount { get; set; }
        public string CurrencyType { get; set; }
        public string AdditionalData { get; set; }
    }
}
