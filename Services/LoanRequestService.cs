using Banking.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class LoanRequestService : ILoanRequestService
{
    private readonly ILoanRequestRepository _loanRequestRepository;

    public LoanRequestService(ILoanRequestRepository loanRequestRepository)
    {
        _loanRequestRepository = loanRequestRepository;
    }

    public async Task<LoanRequest> AddLoanRequestAsync(LoanRequest loanRequest)
    {
        await _loanRequestRepository.AddAsync(loanRequest);
        return loanRequest;
    }

    public async Task<LoanRequest> GetLoanRequestByIdAsync(int loanRequestId)
    {
        return await _loanRequestRepository.GetByIdAsync(loanRequestId);
    }

    public async Task<IEnumerable<LoanRequest>> GetAllLoanRequestsAsync()
    {
        return await _loanRequestRepository.GetAllAsync();
    }

    public async Task<LoanRequest> UpdateLoanRequestAsync(int loanRequestId, LoanRequest loanRequest)
    {
        await _loanRequestRepository.UpdateAsync(loanRequest);
        return loanRequest;
    }

    public async Task<bool> DeleteLoanRequestAsync(int loanRequestId)
    {
        var loanRequest = await _loanRequestRepository.GetByIdAsync(loanRequestId);
        if (loanRequest == null) return false;

        await _loanRequestRepository.DeleteAsync(loanRequestId);
        return true;
    }
}