using Banking.Models;
using Banking.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly BankContext _context;

    public BankAccountService(IBankAccountRepository bankAccountRepository, BankContext context)
    {
        _bankAccountRepository = bankAccountRepository;
        _context = context;
    }

    public async Task<BankAccount> AddBankAccountAsync(BankAccount bankAccount)
    {
        // Generate unique account number if not provided
        if (string.IsNullOrWhiteSpace(bankAccount.AccountNumber))
        {
            bankAccount.AccountNumber = await GenerateUniqueAccountNumberAsync();
        }

        await _bankAccountRepository.AddAsync(bankAccount);
        return bankAccount;
    }

    public async Task<string> GenerateUniqueAccountNumberAsync()
    {
        const int maxRetries = 5;
        int retries = 0;

        while (retries < maxRetries)
        {
            var accountNumber = AccountNumberGenerator.Generate(10);

            var exists = await _context.BankAccounts
                .AnyAsync(a => a.AccountNumber == accountNumber);

            if (!exists)
                return accountNumber;

            retries++;
        }

        throw new InvalidOperationException("Failed to generate a unique account number after multiple attempts.");
    }

    public async Task<BankAccount> GetBankAccountByIdAsync(int accountId)
    {
        return await _bankAccountRepository.GetByIdAsync(accountId);
    }

    public async Task<IEnumerable<BankAccount>> GetAllBankAccountsAsync()
    {
        return await _bankAccountRepository.GetAllAsync();
    }

    public async Task<BankAccount> UpdateBankAccountAsync(int accountId, BankAccount bankAccount)
    {
        await _bankAccountRepository.UpdateAsync(bankAccount);
        return bankAccount;
    }

    public async Task<bool> DeleteBankAccountAsync(int accountId)
    {
        var account = await _bankAccountRepository.GetByIdAsync(accountId);
        if (account == null) return false;

        await _bankAccountRepository.DeleteAsync(accountId);
        return true;
    }
}