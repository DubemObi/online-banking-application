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
    // Basic coverage of LoanController endpoints; mocks ILoanService
    public class LoanControllerTests
    {
        [Fact]
        public async Task GetLoan_ReturnsOk_WhenFound()
        {
            var loan = new Loan { Id = 5, PrincipalAmount = 500m, DurationInMonths = 10 };
            var serviceMock = new Mock<ILoanService>();
            serviceMock.Setup(s => s.GetLoanByIdAsync(5)).ReturnsAsync(loan);

            var controller = new LoanController(serviceMock.Object, new NullLogger<LoanController>());
            var result = await controller.GetLoan(5);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetLoan_ReturnsNotFound_WhenMissing()
        {
            var serviceMock = new Mock<ILoanService>();
            serviceMock.Setup(s => s.GetLoanByIdAsync(99)).ReturnsAsync((Loan?)null);

            var controller = new LoanController(serviceMock.Object, new NullLogger<LoanController>());
            var result = await controller.GetLoan(99);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetLoans_ReturnsNotFound_WhenEmpty()
        {
            var serviceMock = new Mock<ILoanService>();
            serviceMock.Setup(s => s.GetAllLoansAsync()).ReturnsAsync(new List<Loan>());

            var controller = new LoanController(serviceMock.Object, new NullLogger<LoanController>());
            var result = await controller.GetLoans();

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task PutLoan_ReturnsBadRequest_OnIdMismatch()
        {
            var serviceMock = new Mock<ILoanService>();
            var controller = new LoanController(serviceMock.Object, new NullLogger<LoanController>());
            var dto = new LoanDTO { Id = 1, DurationInMonths = 5, BankAccountId = 1, UserId = 1 };
            var result = await controller.PutLoan(2, dto);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}