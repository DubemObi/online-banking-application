using System;
using System.Threading.Tasks;
using Banking.Models;
using Moq;
using Xunit;

namespace Banking.Tests.Services
{
    // LoanRequestService contains conditional logic around approving/rejecting
    // loan requests.  Dependencies to repositories and other services are mocked
    // so the various branches can be traversed deterministically.
    public class LoanRequestServiceTests
    {
        [Fact]
        public async Task ApproveLoanRequestAsync_WhenPendingAndActiveAccount_CreatesLoan()
        {
            var repoMock = new Mock<ILoanRequestRepository>();
            var loanSvcMock = new Mock<ILoanService>();
            var acctSvcMock = new Mock<IBankAccountService>();

            var request = new LoanRequest { Id = 1, Status = LoanStatus.Pending, BankAccountId = 2, UserId = 7, PrincipalAmount = 100m, DurationInMonths = 10 };
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(request);
            acctSvcMock.Setup(a => a.GetBankAccountByIdAsync(2))
                .ReturnsAsync(new BankAccount { AccountId = 2, AccountStatus = AccountStatus.Active });
            loanSvcMock.Setup(l => l.AddLoanAsync(It.IsAny<Loan>())).ReturnsAsync((Loan l) => l);
            repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            var service = new LoanRequestService(repoMock.Object, loanSvcMock.Object, acctSvcMock.Object);
            var dto = new LoanApprovalDTO { LoanRequestId = 1, IsApproved = LoanStatus.Approved, Remarks = "ok" };

            var result = await service.ApproveLoanRequestAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(LoanStatus.Approved, request.Status);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            loanSvcMock.Verify(l => l.AddLoanAsync(It.IsAny<Loan>()), Times.Once);
        }

        [Fact]
        public async Task ApproveLoanRequestAsync_WhenNotFound_Throws()
        {
            var repoMock = new Mock<ILoanRequestRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((LoanRequest?)null);

            var service = new LoanRequestService(repoMock.Object, Mock.Of<ILoanService>(), Mock.Of<IBankAccountService>());
            await Assert.ThrowsAsync<Exception>(async () =>
                await service.ApproveLoanRequestAsync(new LoanApprovalDTO { LoanRequestId = 5, IsApproved = LoanStatus.Approved }));
        }

        [Fact]
        public async Task ApproveLoanRequestAsync_WhenAlreadyProcessed_Throws()
        {
            var repoMock = new Mock<ILoanRequestRepository>();
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new LoanRequest { Id = 1, Status = LoanStatus.Approved });

            var service = new LoanRequestService(repoMock.Object, Mock.Of<ILoanService>(), Mock.Of<IBankAccountService>());
            await Assert.ThrowsAsync<Exception>(async () =>
                await service.ApproveLoanRequestAsync(new LoanApprovalDTO { LoanRequestId = 1, IsApproved = LoanStatus.Approved }));
        }

        [Fact]
        public async Task ApproveLoanRequestAsync_WhenAccountNotActive_Throws()
        {
            var repoMock = new Mock<ILoanRequestRepository>();
            var acctSvcMock = new Mock<IBankAccountService>();
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new LoanRequest { Id = 1, Status = LoanStatus.Pending, BankAccountId = 2 });
            acctSvcMock.Setup(a => a.GetBankAccountByIdAsync(2)).ReturnsAsync((BankAccount?)null);

            var service = new LoanRequestService(repoMock.Object, Mock.Of<ILoanService>(), acctSvcMock.Object);

            await Assert.ThrowsAsync<Exception>(async () =>
                await service.ApproveLoanRequestAsync(new LoanApprovalDTO { LoanRequestId = 1, IsApproved = LoanStatus.Approved }));
        }
    }
}