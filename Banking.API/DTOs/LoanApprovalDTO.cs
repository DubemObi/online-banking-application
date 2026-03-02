using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class LoanApprovalDTO
    {
        [Required]
        public int LoanRequestId { get; set; }
        
        [Required]
        public LoanStatus IsApproved { get; set; }
        public string? Remarks { get; set; }
    }
}