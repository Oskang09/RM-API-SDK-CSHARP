using System;

namespace RM_API_SDK_CSHARP.Model.Transaction
{
    public class Transaction
    {
        public Store store { get; set; }
        public Order order { get; set; }
        public Payee payee { get; set; }
        public string ReferenceId { get; set; }
        public string TransactionId { get; set; }
        public string TerminalId { get; set; }
        public string CurrencyType { get; set; }
        public Int64 BalanceAmount { get; set; }
        public string Platform { get; set; }
        public string Method { get; set; }
        public DateTime TransactionAt { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Region { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }

}