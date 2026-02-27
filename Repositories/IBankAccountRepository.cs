using Banking.Models;

public interface IBankAccountRepository
{
    Task<IEnumerable<BankAccount>> GetAllAsync();
    Task<BankAccount> GetByIdAsync(int id);
    Task AddAsync(BankAccount bankAccount);
    Task UpdateAsync(BankAccount bankAccount);
    Task DeleteAsync(int id);
}