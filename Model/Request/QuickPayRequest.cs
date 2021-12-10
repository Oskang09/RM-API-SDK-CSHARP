using System;
using Newtonsoft.Json;

namespace RM_API_SDK_CSHARP.Model.Request
{
    public class QuickPayRequest
    {
        public string AuthCode { get; set; }
        public string IpAddress { get; set; }
        public string StoreId { get; set; }
        public QuickPayRequestOrder Order { get; set; }
        public QuickPayRequestExtraInfo ExtraInfo { get; set; }

    }

    public class QuickPayRequestExtraInfo
    {
        public string Type { get; set; }
        public string Reference { get; set; }
    }

    public class QuickPayRequestOrder
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public Int64 Amount { get; set; }
        public string CurrencyType { get; set; }
        public string AdditionalData { get; set; }
    }
}