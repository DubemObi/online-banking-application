using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class CardRequestDTO
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public CardType CardType { get; set; }
        [Required]
        public CardBrand CardBrand { get; set; }
       
    }
}

