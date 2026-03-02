using Banking.Models;

public interface IBankAccountService
{
    Task<BankAccount> AddBankAccountAsync(BankAccount bankAccount);
    Task<BankAccount> GetBankAccountByIdAsync(int accountId);
    Task<IEnumerable<BankAccount>> GetAllBankAccountsAsync();
    Task<BankAccount> UpdateBankAccountAsync(int accountId, BankAccount bankAccount);
    Task<bool> DeleteBankAccountAsync(int accountId);
}