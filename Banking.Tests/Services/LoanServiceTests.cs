using System.Collections.Generic;
using System.Threading.Tasks;
using Banking.Models;
using Moq;
using Xunit;

namespace Banking.Tests.Services
{
    // LoanService has a little business logic (installment calculation) so we
    // verify that it is invoked and that CRUD behavior is preserved.
    public class LoanServiceTests
    {
        [Fact]
        public async Task AddLoanAsync_CalculatesMonthlyInstallment_AndReturnsLoan()
        {
            var repo = new Mock<ILoanRepository>();
            repo.Setup(r => r.AddAsync(It.IsAny<Loan>())).Returns(Task.CompletedTask).Verifiable();

            var service = new LoanService(repo.Object);
            var loan = new Loan { PrincipalAmount = 1200m, DurationInMonths = 12 };
            // monthly installment should be calculated inside the service

            var result = await service.AddLoanAsync(loan);

            Assert.Equal(loan, result);
            Assert.True(result.MonthlyInstallment > 0);
            repo.Verify(r => r.AddAsync(loan), Times.Once);
        }

        [Fact]
        public async Task DeleteLoanAsync_WhenNotFound_ReturnsFalse()
        {
            var repo = new Mock<ILoanRepository>();
            repo.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((Loan?)null);

            var svc = new LoanService(repo.Object);
            var ok = await svc.DeleteLoanAsync(42);

            Assert.False(ok);
            repo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetAllLoansAsync_UsesRepository()
        {
            var list = new List<Loan> { new Loan { Id = 1 }, new Loan { Id = 2 } };
            var repo = new Mock<ILoanRepository>();
            repo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var svc = new LoanService(repo.Object);
            var result = await svc.GetAllLoansAsync();

            Assert.Same(list, result);
        }
    }
}