using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Banking.Models;

public class CardDTO
{
    public int Id { get; set; }

    [Required]
    public string CardNumber { get; set; }
    [Required]
    public string Last4Digits { get; set; }

    [Required]
    public CardType CardType { get; set; }

    [Required]
    public CardBrand CardBrand { get; set; }

    public DateTime ExpiryDate { get; set; }

    [Required]
    public string CVV { get; set; }
    [Required]
    public string Pin { get; set; }

    // public bool IsActive { get; set; }
    // public bool IsBlocked { get; set; }


    [Required]
    public int UserId { get; set; }


    [Required]
    public int BankAccountId { get; set; }

}

