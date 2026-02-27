using Banking.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;

    public LoanService(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<Loan> AddLoanAsync(Loan loan)
    {
        loan.CalculateMonthlyInstallment();
        await _loanRepository.AddAsync(loan);
        return loan;
    }

    public async Task<Loan> GetLoanByIdAsync(int loanId)
    {
        return await _loanRepository.GetByIdAsync(loanId);
    }

    public async Task<IEnumerable<Loan>> GetAllLoansAsync()
    {
        return await _loanRepository.GetAllAsync();
    }

    public async Task<Loan> UpdateLoanAsync(int loanId, Loan loan)
    {
        loan.CalculateMonthlyInstallment();
         await _loanRepository.UpdateAsync(loan);
         return loan;
    }

    public async Task<bool> DeleteLoanAsync(int loanId)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null) return false;

        await _loanRepository.DeleteAsync(loanId);
        return true;
    }
}