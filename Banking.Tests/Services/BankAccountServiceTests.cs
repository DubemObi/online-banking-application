using System.Collections.Generic;
using System.Threading.Tasks;
using Banking.Models;
using Moq;
using Xunit;

namespace Banking.Tests.Services
{
    // Service layer tests for BankAccountService.  Dependency on repository is mocked,
    // so we verify behavior without hitting a database.
    public class BankAccountServiceTests
    {
        [Fact]
        public async Task AddBankAccountAsync_CallsRepositoryAndReturnsAccount()
        {
            var repoMock = new Mock<IBankAccountRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<BankAccount>())).Returns(Task.CompletedTask).Verifiable();

            var service = new BankAccountService(repoMock.Object);
            var account = new BankAccount { AccountNumber = "12345", AccountName = "Test", UserId = "1" };

            var result = await service.AddBankAccountAsync(account);

            Assert.Equal(account, result);
            repoMock.Verify(r => r.AddAsync(account), Times.Once);
        }

        [Fact]
        public async Task GetBankAccountByIdAsync_ReturnsAccount()
        {
            var account = new BankAccount { AccountId = 5, AccountNumber = "12345", AccountName = "Test" };
            var repoMock = new Mock<IBankAccountRepository>();
            repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(account);

            var service = new BankAccountService(repoMock.Object);
            var result = await service.GetBankAccountByIdAsync(5);

            Assert.Equal(account, result);
        }

        [Fact]
        public async Task DeleteBankAccountAsync_WhenExists_DeletesAndReturnsTrue()
        {
            var account = new BankAccount { AccountId = 7 };
            var repoMock = new Mock<IBankAccountRepository>();
            repoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(account);
            repoMock.Setup(r => r.DeleteAsync(7)).Returns(Task.CompletedTask).Verifiable();

            var service = new BankAccountService(repoMock.Object);
            var result = await service.DeleteBankAccountAsync(7);

            Assert.True(result);
            repoMock.Verify(r => r.DeleteAsync(7), Times.Once);
        }

        [Fact]
        public async Task DeleteBankAccountAsync_WhenNotFound_ReturnsFalse()
        {
            var repoMock = new Mock<IBankAccountRepository>();
            repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((BankAccount?)null);

            var service = new BankAccountService(repoMock.Object);
            var result = await service.DeleteBankAccountAsync(99);

            Assert.False(result);
            repoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}