using System;
using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class BankAccountDTO
    {
        public int AccountId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string AccountNumber { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string AccountName { get; set; } = null!;


        public AccountType AccountType { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public string UserId { get; set; } = null!;
    }
}