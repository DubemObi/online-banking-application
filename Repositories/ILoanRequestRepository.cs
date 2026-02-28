using Banking.Models;

public interface ILoanRequestRepository
{
    Task<IEnumerable<LoanRequest>> GetAllAsync();
    Task<LoanRequest> GetByIdAsync(int id);
    Task SaveChangesAsync();
    Task AddAsync(LoanRequest loanRequest);
    Task UpdateAsync(LoanRequest loanRequest);
    Task DeleteAsync(int id);
}