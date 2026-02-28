using Banking.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.Repositories
{
    public class LoanRequestRepository : ILoanRequestRepository
    {
        private readonly BankContext _context;

        public LoanRequestRepository(BankContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoanRequest>> GetAllAsync() => await _context.LoanRequests.ToListAsync();

        public async Task<LoanRequest> GetByIdAsync(int id) => await _context.LoanRequests.FindAsync(id);

        public async Task AddAsync(LoanRequest loanRequest)
        {
            _context.LoanRequests.Add(loanRequest);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

        public async Task UpdateAsync(LoanRequest loanRequest)
        {
            _context.Entry(loanRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loanRequest = await _context.LoanRequests.FindAsync(id);
            if (loanRequest != null)
            {
                _context.LoanRequests.Remove(loanRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}