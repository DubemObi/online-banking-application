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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public AccountType AccountType { get; set; }
        public AccountStatus AccountStatus { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }
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
