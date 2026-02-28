using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Banking.Models
{

    public class Card
    {
        public int Id { get; set; }

        [Required]
        public string CardNumberHash { get; set; }
        [Required]
        public string Last4Digits { get; set; }

        [Required]
        public CardType CardType { get; set; }

        [Required]
        public CardBrand CardBrand { get; set; }

        public DateTime ExpiryDate { get; set; }

        [Required]
        public string CVVHash { get; set; }

        [Required]
        public string PinHash { get; set; }

        // public bool IsActive { get; set; }
        // public bool IsBlocked { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [Required]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [Required]
        public int BankAccountId { get; set; }

        [JsonIgnore]
        public BankAccount BankAccount { get; set; }


    }
    public enum CardType
    {
        Debit,
        Credit
    }

    public enum CardBrand
    {
        Visa,
        MasterCard
    }
}