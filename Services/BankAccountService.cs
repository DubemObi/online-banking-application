using Banking.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class BankAccountService : IBankAccountService
{
    private readonly BankContext _context;

    public BankAccountService(BankContext context)
    {
        _context = context;
    }

    public async Task<BankAccount> AddBankAccountAsync(BankAccount bankAccount)
    {
        _context.BankAccounts.Add(bankAccount);
        await _context.SaveChangesAsync();
        return bankAccount;
    }

    public async Task<BankAccount> GetBankAccountByIdAsync(int accountId)
    {
        return await _context.BankAccounts.FindAsync(accountId);
    }

    public async Task<IEnumerable<BankAccount>> GetAllBankAccountsAsync()
    {
        return await _context.BankAccounts.ToListAsync();
    }

    public async Task<BankAccount> UpdateBankAccountAsync(int accountId, BankAccount bankAccount)
    {
        var existingAccount = await _context.BankAccounts.FindAsync(accountId);
        if (existingAccount == null) return null;

        existingAccount.AccountNumber = bankAccount.AccountNumber;
        existingAccount.AccountName = bankAccount.AccountName;
        existingAccount.AccountBalance = bankAccount.AccountBalance;
        existingAccount.AccountType = bankAccount.AccountType;
        existingAccount.AccountStatus = bankAccount.AccountStatus;

        await _context.SaveChangesAsync();
        return existingAccount;
    }

    public async Task<bool> DeleteBankAccountAsync(int accountId)
    {
        var account = await _context.BankAccounts.FindAsync(accountId);
        if (account == null) return false;

        _context.BankAccounts.Remove(account);
        await _context.SaveChangesAsync();
        return true;
    }
}