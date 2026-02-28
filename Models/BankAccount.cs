using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Banking.Models
{
    public class BankAccount
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string AccountNumber { get; set; }
        [Required]
        [StringLength(100)]
        public string AccountName { get; set; }
        public decimal AccountBalance { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public AccountType AccountType { get; set; } = AccountType.Savings;
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

        public int UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }

        // public ICollection<Transaction> IncomingTransactions { get; set; }


        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Invalid deposit amount");

            AccountBalance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Invalid withdrawal amount");

            if (AccountBalance < amount)
                throw new InvalidOperationException("Insufficient funds");

            AccountBalance -= amount;
        }
    }

    public enum AccountType
    {
        Checking = 1,
        Savings = 2,
        // Business = 3
    }

    // Apply interest on savings accounts monthly 
    // and lower interest on checking accounts.

    public enum AccountStatus
    {
        Active = 1,
        Frozen = 2,
        Closed = 3
    }
}
