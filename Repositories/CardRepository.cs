using Banking.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.Repositories
{
    public class CardRequestRepository : ICardRequestRepository
    {
        private readonly BankContext _context;

        public CardRequestRepository(BankContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CardRequest>> GetAllAsync() => await _context.CardRequests.ToListAsync();

        public async Task<CardRequest> GetByIdAsync(int id) => await _context.CardRequests.FindAsync(id);

        public async Task AddAsync(CardRequest cardRequest)
        {
            _context.CardRequests.Add(cardRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CardRequest cardRequest)
        {
            _context.Entry(cardRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cardRequest = await _context.CardRequests.FindAsync(id);
            if (cardRequest != null)
            {
                _context.CardRequests.Remove(cardRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}