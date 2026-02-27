using Banking.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountService(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<BankAccount> AddBankAccountAsync(BankAccount bankAccount)
    {
        await _bankAccountRepository.AddAsync(bankAccount);
        return bankAccount;
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