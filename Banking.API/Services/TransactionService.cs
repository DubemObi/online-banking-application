using System.Threading.Tasks;
using Banking.Models;
using Banking.Repositories;
using Microsoft.EntityFrameworkCore;

public class TransactionService : ITransactionService
{
    private readonly BankContext _context;

    public TransactionService(BankContext context)
    {
        _context = context;
    }

    public async Task DepositAsync(int accountId, decimal amount)
    {
        using var dbTransaction = await _context.Database.BeginTransactionAsync();

        var account = await _context.BankAccounts.FindAsync(accountId);
        if (account == null)
            throw new Exception("Account not found");

        account.Deposit(amount);

        var transaction = new Transaction
        {
            AccountId = account.AccountId,
            Amount = amount,
            TransactionType = TransactionType.Deposit,
            Status = TransactionStatus.Completed,
            Description = "Deposit transaction",
            BankAccount = account
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        await dbTransaction.CommitAsync();

    }

    public async Task WithdrawAsync(int accountId, decimal amount)
    {
        using var dbTransaction = await _context.Database.BeginTransactionAsync();

        var account = await _context.BankAccounts.FindAsync(accountId);
        if (account == null)
            throw new Exception("Account not found");

        account.Withdraw(amount);

        _context.Transactions.Add(new Transaction
        {
            AccountId = account.AccountId,
            Amount = amount,
            TransactionType = TransactionType.Withdrawal,
            Status = TransactionStatus.Completed,
            Description = "Withdrawal transaction",
            BankAccount = account
        });

        await _context.SaveChangesAsync();
        await dbTransaction.CommitAsync();
    }

    public async Task TransferAsync(int fromAccountId, string? toAccountNumber, decimal amount)
    {
        using var dbTransaction = await _context.Database.BeginTransactionAsync();

        var fromAccount = await _context.BankAccounts
            .FirstOrDefaultAsync(a => a.AccountId == fromAccountId);

        var toAccount = await _context.BankAccounts
            .FirstOrDefaultAsync(a => a.AccountNumber == toAccountNumber);

        if (fromAccount == null || toAccount == null)
            throw new Exception("Account not found");

        if (fromAccount.AccountBalance < amount)
            throw new Exception("Insufficient funds");

        // Debit
        fromAccount.Withdraw(amount);

        // Credit
        toAccount.Deposit(amount);

        var reference = Guid.NewGuid().ToString();

        _context.Transactions.AddRange(
            new Transaction
            {
                AccountId = fromAccount.AccountId,
                Amount = amount,
                TransactionType = TransactionType.TransferDebit,
                RecipientAccountId = toAccount.AccountId,
                Description = $"Transfer to account {toAccount.AccountNumber}",
                Reference = reference,
                BankAccount = fromAccount,
                RecipientAccount = toAccount
            },
            new Transaction
            {
                AccountId = toAccount.AccountId,
                Amount = amount,
                TransactionType = TransactionType.TransferCredit,
                RecipientAccountId = fromAccount.AccountId,
                Description = $"Transfer from account {fromAccount.AccountNumber}",
                Reference = reference

            }
        );

        await _context.SaveChangesAsync();
        await dbTransaction.CommitAsync();
    }
    public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
    {
        return await _context.Transactions.FindAsync(transactionId);
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
        return await _context.Transactions.ToListAsync();
    }
}