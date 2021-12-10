using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RM_API_SDK_CSHARP.Model.Transaction
{
    public class OnlineTransaction
    {
        public string Id { get; set; }
        public Order order { get; set; }
        public string Type { get; set; }
        public string TransactionId { get; set; }
        public string Platform { get; set; }
        public List<String> Method { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectURL { get; set; }

        [JsonProperty("notifyUrl")]
        public String NotifyURL { get; set; }
        public string StartAt { get; set; }
        public string EndAt { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}