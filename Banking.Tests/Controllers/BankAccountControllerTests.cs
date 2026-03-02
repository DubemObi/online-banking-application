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
    // Controller tests for BankAccountController.  Service dependency is mocked
    // to isolate the controller's behavior and verify returned action results.
    public class BankAccountControllerTests
    {
        [Fact]
        public async Task GetBankAccount_ReturnsOk_WhenFound()
        {
            var account = new BankAccount { AccountId = 3, AccountNumber = "A1", AccountName = "Acct" };
            var serviceMock = new Mock<IBankAccountService>();
            serviceMock.Setup(s => s.GetBankAccountByIdAsync(3)).ReturnsAsync(account);

            var controller = new BankAccountController(serviceMock.Object, new NullLogger<BankAccountController>());

            var actionResult = await controller.GetBankAccount(3);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var ok = actionResult.Result as OkObjectResult;
            Assert.Equal(account, ok.Value);
        }

        [Fact]
        public async Task GetBankAccount_ReturnsNotFound_WhenMissing()
        {
            var serviceMock = new Mock<IBankAccountService>();
            serviceMock.Setup(s => s.GetBankAccountByIdAsync(4)).ReturnsAsync((BankAccount?)null);

            var controller = new BankAccountController(serviceMock.Object, new NullLogger<BankAccountController>());

            var actionResult = await controller.GetBankAccount(4);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetBankAccounts_ReturnsNotFound_WhenEmpty()
        {
            var serviceMock = new Mock<IBankAccountService>();
            serviceMock.Setup(s => s.GetAllBankAccountsAsync()).ReturnsAsync(new List<BankAccount>());

            var controller = new BankAccountController(serviceMock.Object, new NullLogger<BankAccountController>());

            var actionResult = await controller.GetBankAccounts();
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task PutBankAccount_ReturnsBadRequest_OnIdMismatch()
        {
            var serviceMock = new Mock<IBankAccountService>();
            var controller = new BankAccountController(serviceMock.Object, new NullLogger<BankAccountController>());

            var dto = new BankAccountDTO { AccountId = 2, AccountNumber = "X", AccountName = "Y", UserId = 1 };
            var result = await controller.PutBankAccount(3, dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PostBankAccount_ReturnsCreatedAtAction_WhenValid()
        {
            var serviceMock = new Mock<IBankAccountService>();
            serviceMock.Setup(s => s.AddBankAccountAsync(It.IsAny<BankAccount>())).ReturnsAsync((BankAccount a) => a);

            var controller = new BankAccountController(serviceMock.Object, new NullLogger<BankAccountController>());

            var dto = new BankAccountDTO { AccountNumber = "98765", AccountName = "New", UserId = 1 };
            var result = await controller.PostBankAccount(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(dto, created.Value);
        }

        [Fact]
        public async Task DeleteBankAccount_ReturnsNoContent_WhenDeleted()
        {
            var account = new BankAccount { AccountId = 10 };
            var serviceMock = new Mock<IBankAccountService>();
            serviceMock.Setup(s => s.GetBankAccountByIdAsync(10)).ReturnsAsync(account);
            serviceMock.Setup(s => s.DeleteBankAccountAsync(10)).ReturnsAsync(true);

            var controller = new BankAccountController(serviceMock.Object, new NullLogger<BankAccountController>());
            var result = await controller.DeleteBankAccount(10);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBankAccount_ReturnsNotFound_WhenMissing()
        {
            var serviceMock = new Mock<IBankAccountService>();
            serviceMock.Setup(s => s.GetBankAccountByIdAsync(11)).ReturnsAsync((BankAccount?)null);

            var controller = new BankAccountController(serviceMock.Object, new NullLogger<BankAccountController>());
            var result = await controller.DeleteBankAccount(11);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}