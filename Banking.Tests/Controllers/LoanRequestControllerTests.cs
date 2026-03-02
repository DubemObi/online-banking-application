using System.Collections.Generic;
using System.Threading.Tasks;
using Banking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using banking.Controllers;

namespace Banking.Tests.Controllers
{
    public class LoanRequestControllerTests
    {
        [Fact]
        public async Task GetLoanRequest_ReturnsOk_WhenFound()
        {
            var req = new LoanRequest { Id = 1 };
            var svc = new Mock<ILoanRequestService>();
            svc.Setup(s => s.GetLoanRequestByIdAsync(1)).ReturnsAsync(req);
            var controller = new LoanRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<LoanRequestController>());

            var res = await controller.GetLoanRequest(1);
            var ok = Assert.IsType<OkObjectResult>(res.Result);
            Assert.Equal(req, ok.Value);
        }

        [Fact]
        public async Task GetLoanRequest_ReturnsNotFound_WhenMissing()
        {
            var svc = new Mock<ILoanRequestService>();
            svc.Setup(s => s.GetLoanRequestByIdAsync(2)).ReturnsAsync((LoanRequest?)null);
            var controller = new LoanRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<LoanRequestController>());

            var res = await controller.GetLoanRequest(2);
            Assert.IsType<NotFoundObjectResult>(res.Result);
        }

        [Fact]
        public async Task GetLoanRequests_ReturnsNotFound_WhenEmpty()
        {
            var svc = new Mock<ILoanRequestService>();
            svc.Setup(s => s.GetAllLoanRequestsAsync()).ReturnsAsync(new List<LoanRequest>());
            var controller = new LoanRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<LoanRequestController>());
            var res = await controller.GetLoanRequests();
            Assert.IsType<NotFoundObjectResult>(res.Result);
        }

        [Fact]
        public async Task PostLoanRequest_ReturnsCreated_WhenValid()
        {
            var svc = new Mock<ILoanRequestService>();
            svc.Setup(s => s.AddLoanRequestAsync(It.IsAny<LoanRequest>())).ReturnsAsync((LoanRequest r) => r);
            var bankSvc = new Mock<IBankAccountService>();
            bankSvc.Setup(b => b.GetBankAccountByIdAsync(It.IsAny<int>())).ReturnsAsync(new BankAccount { AccountStatus = AccountStatus.Active });

            var controller = new LoanRequestController(svc.Object, bankSvc.Object, new NullLogger<LoanRequestController>());
            var dto = new LoanRequestDTO { BankAccountId = 5, PrincipalAmount = 1000m, DurationInMonths = 12 };
            var res = await controller.PostLoanRequest(dto);
            Assert.IsType<CreatedAtActionResult>(res.Result);
        }

        [Fact]
        public async Task PutLoanRequest_BadRequest_OnIdMismatch()
        {
            var svc = new Mock<ILoanRequestService>();
            var controller = new LoanRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<LoanRequestController>());
            var dto = new LoanRequestDTO { Id = 3 };
            var result = await controller.PutLoanRequest(4, dto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ApproveLoan_ReturnsOk_WhenRejected()
        {
            var svc = new Mock<ILoanRequestService>();
            svc.Setup(s => s.ApproveLoanRequestAsync(It.IsAny<LoanApprovalDTO>())).ReturnsAsync((Loan)null);
            var controller = new LoanRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<LoanRequestController>());
            var dto = new LoanApprovalDTO { LoanRequestId = 1, IsApproved = LoanStatus.Rejected };
            var res = await controller.ApproveLoan(dto);
            var ok = Assert.IsType<OkObjectResult>(res);
            Assert.Equal("Loan rejected successfully", ok.Value);
        }

        [Fact]
        public async Task ApproveLoan_ReturnsOk_WhenApproved()
        {
            var svc = new Mock<ILoanRequestService>();
            svc.Setup(s => s.ApproveLoanRequestAsync(It.IsAny<LoanApprovalDTO>())).ReturnsAsync(new Loan());
            var controller = new LoanRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<LoanRequestController>());
            var dto = new LoanApprovalDTO { LoanRequestId = 1, IsApproved = LoanStatus.Approved };
            var res = await controller.ApproveLoan(dto);
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async Task DeleteLoanRequest_ReturnsNotFound_WhenMissing()
        {
            var svc = new Mock<ILoanRequestService>();
            svc.Setup(s => s.GetLoanRequestByIdAsync(9)).ReturnsAsync((LoanRequest?)null);
            var controller = new LoanRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<LoanRequestController>());
            var res = await controller.DeleteLoanRequest(9);
            Assert.IsType<NotFoundObjectResult>(res);
        }
    }
}
