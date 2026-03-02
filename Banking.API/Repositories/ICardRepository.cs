using Banking.Models;

public interface ICardRepository
{
    Task<IEnumerable<Card>> GetAllAsync();
    Task<Card> GetByIdAsync(int id);
    Task AddAsync(Card card);
    Task UpdateAsync(Card card);
    Task DeleteAsync(int id);
}