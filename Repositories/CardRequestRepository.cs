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

        public async Task AddAsync(CardRequest card)
        {
            _context.CardRequests.Add(card);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CardRequest card)
        {
            _context.Entry(card).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card != null)
            {
                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();
            }
        }
    }
}