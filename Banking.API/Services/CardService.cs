using Banking.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;

    public CardService(ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
    }

    public async Task<Card> AddCardAsync(Card card)
    {
        await _cardRepository.AddAsync(card);
        return card;
    }

    public async Task<Card> GetCardByIdAsync(int cardId)
    {
        return await _cardRepository.GetByIdAsync(cardId);
    }

    public async Task<IEnumerable<Card>> GetAllCardsAsync()
    {
        return await _cardRepository.GetAllAsync();
    }

    public async Task<Card> UpdateCardAsync(int cardId, Card card)
    {
        await _cardRepository.UpdateAsync(card);
        return card;
    }

    public async Task<bool> DeleteCardAsync(int cardId)
    {
        var card = await _cardRepository.GetByIdAsync(cardId);
        if (card == null) return false;

        await _cardRepository.DeleteAsync(cardId);
        return true;
    }
}