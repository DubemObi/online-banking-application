using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class CardApprovalDTO
    {
        [Required]
        public int CardRequestId { get; set; }
        
        [Required]
        public CardRequestStatus IsApproved { get; set; }
        public string? Remarks { get; set; }
    }
}