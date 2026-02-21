using System.Collections.Generic;

namespace Banking.Models
{
    public class BankAccount
    {
        // public int BankAccountId { get; set; }
        public string AccountID {get; set;}
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public double AccountBalance {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

        public AccountType AccountType {get; set;}
        public AccountStatus AccountStatus {get; set;}

        public int UserId { get; set;  }
        public User User {get; set;}
    }

    public enum AccountType{
    Checking = 1,
    Savings = 2,
    Business = 3
}

public enum AccountStatus
{
    Active = 1,
    Frozen = 2,
    Closed = 3
}
}
