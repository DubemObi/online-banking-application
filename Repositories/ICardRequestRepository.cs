using Banking.Models;

public interface ICardRequestRepository
{
    Task<IEnumerable<CardRequest>> GetAllAsync();
    Task<CardRequest> GetByIdAsync(int id);
    Task AddAsync(CardRequest cardRequest);
    Task UpdateAsync(CardRequest cardRequest);
    Task DeleteAsync(int id);
}