
using System;
using RM_API_SDK_CSHARP.Model.Transaction;

namespace RM_API_SDK_CSHARP.Model.Response
{
    public class QuickPayResult
    {
        public Store Store { get; set; }
        public string ReferenceId { get; set; }
        public string TransactionId { get; set; }
        public Order Order { get; set; }
        public string TerminalId { get; set; }
        public Payee Payee { get; set; }
        public string CurrencyType { get; set; }
        public Int64 BalanceAmount { get; set; }
        public string Platform { get; set; }
        public string Method { get; set; }
        public string Type { get; set; }
        public string Region { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

