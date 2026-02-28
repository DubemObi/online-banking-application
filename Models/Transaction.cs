using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Transactions;

namespace Banking.Models
{
    public class Transaction
    {
        [Required]
        public int TransactionId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public string? Reference { get; set; }


        [Required]
        public int AccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        // [JsonIgnore]
        public int? RecipientAccountId { get; set; }
        public BankAccount? RecipientAccount { get; set; }

    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed,
        Reversed
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        TransferDebit,
        TransferCredit
    }
}
