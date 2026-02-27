using Banking.Models;

public interface ILoanService
{
    Task<Loan> AddLoanAsync(Loan loan);
    Task<Loan> GetLoanByIdAsync(int loanId);
    Task<IEnumerable<Loan>> GetAllLoansAsync();
    Task<Loan> UpdateLoanAsync(int loanId, Loan loan);
    Task<bool> DeleteLoanAsync(int loanId);
}