using Banking.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class LoanRequestService : ILoanRequestService
{
    private readonly ILoanRequestRepository _loanRequestRepository;
    private readonly ILoanService _loanService;
    private readonly IBankAccountService _bankAccountService;

    public LoanRequestService(ILoanRequestRepository loanRequestRepository, ILoanService loanService, IBankAccountService bankAccountService)
    {
        _loanRequestRepository = loanRequestRepository;
        _loanService = loanService;
        _bankAccountService = bankAccountService;
    }

    public async Task<LoanRequest> AddLoanRequestAsync(LoanRequest loanRequest)
    {
        await _loanRequestRepository.AddAsync(loanRequest);
        return loanRequest;
    }

    public async Task<Loan> ApproveLoanRequestAsync(LoanApprovalDTO dto)
    {
        var request = await _loanRequestRepository.GetByIdAsync(dto.LoanRequestId);
        if (request == null) throw new Exception("Loan request not found");
        if (request.Status != LoanStatus.Pending) throw new Exception("Already processed");

        var bankAccount = await _bankAccountService.GetBankAccountByIdAsync(request.BankAccountId);

        if (bankAccount == null || bankAccount.AccountStatus != AccountStatus.Active)
                    throw new Exception("User must have an active bank account to request a loan");

        request.Status = dto.IsApproved == LoanStatus.Approved ? LoanStatus.Approved : LoanStatus.Rejected;
        request.ReviewedAt = DateTime.UtcNow;
        request.AdminRemarks = dto.Remarks;

        if (dto.IsApproved != LoanStatus.Approved)
        {
            await _loanRequestRepository.SaveChangesAsync();
            return null;
        }

        // Create actual loan
        var loan = new Loan
        {
            UserId = request.UserId,
            PrincipalAmount = request.PrincipalAmount,
            DurationInMonths = request.DurationInMonths,
            Status = LoanStatus.Approved,
            CreatedAt = DateTime.UtcNow
        };

        await _loanService.AddLoanAsync(loan);
        await _loanRequestRepository.SaveChangesAsync();
        return loan;
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