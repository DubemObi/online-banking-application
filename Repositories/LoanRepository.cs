using Banking.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly BankContext _context;

        public LoanRepository(BankContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetAllAsync() => await _context.Loans.ToListAsync();

        public async Task<Loan> GetByIdAsync(int id) => await _context.Loans.FindAsync(id);

        public async Task AddAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Loan loan)
        {
            _context.Entry(loan).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
            }
        }
    }
}