using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class CardRequest
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public CardType CardType { get; set; }
        [Required]
        public CardBrand CardBrand { get; set; }
        [Required]
        public CardRequestStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }
    }

    public enum CardRequestStatus
    {
        Pending,
        Approved,
        Rejected
    }
}