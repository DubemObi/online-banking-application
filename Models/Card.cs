namespace Banking.Models 
{

public class Card
{
    public int Id { get; set; }
  
    public string CardNumberHash { get; set; } 
    public string Last4Digits { get; set; }
    public CardType CardType { get; set; }       
    public CardBrand CardBrand { get; set; }     

    public DateTime ExpiryDate { get; set; }

    public string CVVHash { get; set; }        
    public string PinHash { get; set; }

    public bool IsActive { get; set; }
    public bool IsBlocked { get; set; }

    public DateTime CreatedAt { get; set; }


    public int UserId { get; set; }          
    public User User { get; set; }
    public int BankAccountId { get; set; }     
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