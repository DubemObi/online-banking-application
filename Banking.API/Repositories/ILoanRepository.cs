using Banking.Models;

public interface ILoanRepository
{
    Task<IEnumerable<Loan>> GetAllAsync();
    Task<Loan> GetByIdAsync(int id);
    Task AddAsync(Loan loan);
    Task UpdateAsync(Loan loan);
    Task DeleteAsync(int id);
}