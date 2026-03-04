using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Banking.Models;


public class CardResponseDTO
{
    public int Id { get; set; }

    public string CardNumberHash { get; set; }
    public string Last4Digits { get; set; }

    public CardType CardType { get; set; }

    public CardBrand CardBrand { get; set; }

    public DateTime ExpiryDate { get; set; }
    public string CVVHash { get; set; }

    public string PinHash { get; set; }


    public DateTime CreatedAt { get; set; }


    public string UserId { get; set; } = null!;
    public int BankAccountId { get; set; }


}
