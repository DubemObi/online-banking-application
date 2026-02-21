using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Transactions;

namespace Banking.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string SourceAccount { get; set; }
        public string DestAccount { get; set; }
        public double Amount { get; set; }
        public TransactionStatus Status {get; set;}
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    

        [JsonIgnore]
        public List<BankAccount>? BankAccounts { get; set; }
    }

    public enum TransactionStatus
    {
        
    }
}
