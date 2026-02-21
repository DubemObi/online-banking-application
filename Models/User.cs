using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Banking.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password {get; set;}
        public int MobileNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    

        [JsonIgnore]
        public List<BankAccount>? BankAccounts { get; set; }
    }
}
