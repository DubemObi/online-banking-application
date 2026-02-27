using Banking.Models;

public interface ICardRequestService
{
    Task<CardRequest> AddCardRequestAsync(CardRequest cardRequest);
    Task<CardRequest> GetCardRequestByIdAsync(int cardRequestId);
    Task<IEnumerable<CardRequest>> GetAllCardRequestsAsync();
    Task<CardRequest> UpdateCardRequestAsync(int cardRequestId, CardRequest cardRequest);
    Task<bool> DeleteCardRequestAsync(int cardRequestId);
}