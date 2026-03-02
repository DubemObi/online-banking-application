using Banking.Models;

public interface ICardService
{
    Task<Card> AddCardAsync(Card card);
    Task<Card> GetCardByIdAsync(int cardId);
    Task<IEnumerable<Card>> GetAllCardsAsync();
    Task<Card> UpdateCardAsync(int cardId, Card card);
    Task<bool> DeleteCardAsync(int cardId);
}