using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class LoanRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "999999999999")]
        public decimal PrincipalAmount { get; set; }

        [Required]
        [Range(1, 600)]
        public int DurationInMonths { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public int BankAccountId { get; set; }
    }
     
}

