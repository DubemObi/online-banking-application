using Banking.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly BankContext _context;

        public BankAccountRepository(BankContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BankAccount>> GetAllAsync() => await _context.BankAccounts.ToListAsync();

        public async Task<BankAccount> GetByIdAsync(int id) => await _context.BankAccounts.FindAsync(id);

        public async Task AddAsync(BankAccount bankAccount)
        {
            _context.BankAccounts.Add(bankAccount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BankAccount bankAccount)
        {
            _context.Entry(bankAccount).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount != null)
            {
                _context.BankAccounts.Remove(bankAccount);
                await _context.SaveChangesAsync();
            }
        }
    }
}