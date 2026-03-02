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
    // Minimal coverage of TransactionController endpoints.
    public class TransactionControllerTests
    {
        [Fact]
        public async Task GetTransaction_ReturnsNotFound_WhenMissing()
        {
            var svc = new Mock<ITransactionService>();
            svc.Setup(s => s.GetTransactionByIdAsync(7)).ReturnsAsync((Transaction?)null);

            var controller = new TransactionController(svc.Object, new NullLogger<TransactionController>());
            var result = await controller.GetTransaction(7);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task DepositTransaction_ReturnsCreated_WhenValid()
        {
            var svc = new Mock<ITransactionService>();
            svc.Setup(s => s.DepositAsync(It.IsAny<int>(), It.IsAny<decimal>())).Returns(Task.CompletedTask);

            var controller = new TransactionController(svc.Object, new NullLogger<TransactionController>());
            var dto = new TransactionDTO { AccountId = 1, Amount = 50m, Status = TransactionStatus.Completed, TransactionType = TransactionType.Deposit };

            var result = await controller.DepositTransaction(dto);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }
    }
}