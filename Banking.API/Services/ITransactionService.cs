using Banking.Models;

public interface ITransactionService
{
    Task DepositAsync(int accountId, decimal amount);
    Task WithdrawAsync(int accountId, decimal amount);
    Task TransferAsync(int fromAccountId, string? toAccountNumber, decimal amount);
    Task<Transaction> GetTransactionByIdAsync(int transactionId);
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    // Task<Transaction> UpdateTransactionAsync(int transactionId, Transaction transaction);
    // Task<bool> DeleteTransactionAsync(int transactionId);
}