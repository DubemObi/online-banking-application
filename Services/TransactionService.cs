using System.Threading.Tasks;
using Banking.Models;
using Banking.Repositories;
using Microsoft.EntityFrameworkCore;

public class TransactionService
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

    _context.Transactions.Add(new Transaction
    {
        AccountId = account.AccountId,
        Amount = amount,
        TransactionType = TransactionType.Deposit,
        Description = "Deposit transaction",
        BankAccount = account
    });

    await _context.SaveChangesAsync();
    await dbTransaction.CommitAsync();
}

    public async Task WithdrawAsync(Guid accountId, decimal amount)
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
        Description = "Withdrawal transaction",
        BankAccount = account
    });

    await _context.SaveChangesAsync();
    await dbTransaction.CommitAsync();
}

    public async Task TransferAsync(int fromAccountId, int toAccountId, decimal amount)
{
    using var dbTransaction = await _context.Database.BeginTransactionAsync();

    var fromAccount = await _context.BankAccounts
        .FirstOrDefaultAsync(a => a.AccountId == fromAccountId);

    var toAccount = await _context.BankAccounts
        .FirstOrDefaultAsync(a => a.AccountId == toAccountId);

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
}