using Banking.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly BankContext _context;

        public CardRepository(BankContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Card>> GetAllAsync() => await _context.Cards.ToListAsync();

        public async Task<Card> GetByIdAsync(int id) => await _context.Cards.FindAsync(id);

        public async Task AddAsync(Card card)
        {
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Card card)
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