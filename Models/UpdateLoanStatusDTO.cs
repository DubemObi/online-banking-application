using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class UpdateLoanStatusDTO
    {
        [Required]
        public LoanStatus Status { get; set; }
    }
}