using Banking.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class CardRequestService : ICardRequestService
{
    private readonly ICardRequestRepository _cardRequestRepository;

    public CardRequestService(ICardRequestRepository cardRequestRepository)
    {
        _cardRequestRepository = cardRequestRepository;
    }

    public async Task<CardRequest> AddCardRequestAsync(CardRequest cardRequest)
    {
        await _cardRequestRepository.AddAsync(cardRequest);
        return cardRequest;
    }

    public async Task<CardRequest> GetCardRequestByIdAsync(int cardRequestId)
    {
        return await _cardRequestRepository.GetByIdAsync(cardRequestId);
    }

    public async Task<IEnumerable<CardRequest>> GetAllCardRequestsAsync()
    {
        return await _cardRequestRepository.GetAllAsync();
    }

    public async Task<CardRequest> UpdateCardRequestAsync(int cardRequestId, CardRequest cardRequest)
    {
        await _cardRequestRepository.UpdateAsync(cardRequest);
        return cardRequest;
    }

    public async Task<bool> DeleteCardRequestAsync(int cardRequestId)
    {
        var cardRequest = await _cardRequestRepository.GetByIdAsync(cardRequestId);
        if (cardRequest == null) return false;

        await _cardRequestRepository.DeleteAsync(cardRequestId);
        return true;
    }
}