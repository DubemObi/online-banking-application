using Banking.Models;

public interface ILoanRequestService
{
    Task<LoanRequest> AddLoanRequestAsync(LoanRequest loanRequest);
    Task<LoanRequest> GetLoanRequestByIdAsync(int loanRequestId);
    Task<IEnumerable<LoanRequest>> GetAllLoanRequestsAsync();
    Task<LoanRequest> UpdateLoanRequestAsync(int loanRequestId, LoanRequest loanRequest);
    Task<bool> DeleteLoanRequestAsync(int loanRequestId);
}