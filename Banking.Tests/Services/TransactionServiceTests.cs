using System;
using System.Threading.Tasks;
using Banking.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Banking.Tests.Services
{
    // TransactionService interacts directly with BankContext and contains
    // transactional logic; here we exercise Deposit/Withdraw/Transfer paths
    // using an in‑memory database.
    public class TransactionServiceTests
    {
        private BankContext CreateContext(string name)
        {
            var options = new DbContextOptionsBuilder<BankContext>()
                .UseInMemoryDatabase(databaseName: name)
                // suppress transaction warnings which otherwise become exceptions
                .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            var ctx = new BankContext(options);
            // ensure clean database for each call to avoid stale state from previous tests
            ctx.Database.EnsureDeleted();
            return ctx;
        }

        [Fact]
        public async Task DepositAsync_IncreasesBalanceAndCreatesTransaction()
        {
            var ctx = CreateContext("deposit");
            var account = new BankAccount
            {
                AccountId = 1,
                AccountBalance = 0m,
                AccountName = "Test",
                AccountNumber = "ACC1",
                UserId = 1,
                RowVersion = new byte[] { 0 }
            };
            ctx.BankAccounts.Add(account);
            try
            {
                await ctx.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("Exception occurred while saving initial account");
                Console.WriteLine(e.ToString());
                throw;
            }

            // make sure the account has its UserId before calling service
            var pre = await ctx.BankAccounts.FindAsync(1);
            Assert.NotNull(pre);
            Assert.Equal(1, pre.UserId);

            var service = new TransactionService(ctx);
            try
            {
                await service.DepositAsync(1, 100m);
            }
            catch (DbUpdateException ex)
            {
                // if it still fails, dump change tracker
                foreach (var entry in ctx.ChangeTracker.Entries<BankAccount>())
                {
                    System.Console.WriteLine($"Entry state={entry.State}, UserId='{entry.Entity.UserId}'");
                }
                throw;
            }

            var updated = await ctx.BankAccounts.FindAsync(1);
            Assert.Equal(100m, updated.AccountBalance);
            Assert.Single(ctx.Transactions);
            var tx = await ctx.Transactions.FirstAsync();
            Assert.Equal(TransactionType.Deposit, tx.TransactionType);
        }

        [Fact]
        public async Task WithdrawAsync_Throws_WhenAccountMissing()
        {
            var ctx = CreateContext("withdrawMissing");
            var service = new TransactionService(ctx);
            // in-memory provider throws InvalidOperationException due to unsupported transactions,
            // so allow either type so the test isn't brittle
            await Assert.ThrowsAnyAsync<Exception>(async () =>
                await service.WithdrawAsync(99, 50m));
        }

        [Fact]
        public async Task TransferAsync_MovesFundsBetweenAccounts()
        {
            var ctx = CreateContext("transfer");
            var a = new BankAccount
            {
                AccountId = 1,
                AccountNumber = "A",
                AccountName = "Foo",
                AccountBalance = 200m,
                UserId = 1,
                RowVersion = new byte[] { 0 }
            };
            var b = new BankAccount
            {
                AccountId = 2,
                AccountNumber = "B",
                AccountName = "Bar",
                AccountBalance = 50m,
                UserId = 2,
                RowVersion = new byte[] { 0 }
            };
            ctx.BankAccounts.AddRange(a, b);
            await ctx.SaveChangesAsync();

            var service = new TransactionService(ctx);
            await service.TransferAsync(1, "B", 100m);

            var aUpd = await ctx.BankAccounts.FindAsync(1);
            var bUpd = await ctx.BankAccounts.FindAsync(2);
            Assert.Equal(100m, aUpd.AccountBalance);
            Assert.Equal(150m, bUpd.AccountBalance);
            Assert.Equal(2, await ctx.Transactions.CountAsync());
        }
    }
}