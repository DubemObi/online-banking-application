using Banking.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<BankAccount> BankAccounts { get; set; }
        public ICollection<Card> Cards { get; set; }
        public ICollection<Loan> Loans { get; set; }

}