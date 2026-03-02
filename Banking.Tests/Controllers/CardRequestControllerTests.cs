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
    public class CardRequestControllerTests
    {
        [Fact]
        public async Task GetCardRequest_ReturnsOk_WhenFound()
        {
            var req = new CardRequest { Id = 1 };
            var svc = new Mock<ICardRequestService>();
            svc.Setup(s => s.GetCardRequestByIdAsync(1)).ReturnsAsync(req);
            var controller = new CardRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<CardRequestController>());

            var res = await controller.GetCardRequest(1);
            var ok = Assert.IsType<OkObjectResult>(res.Result);
            Assert.Equal(req, ok.Value);
        }

        [Fact]
        public async Task GetCardRequest_ReturnsNotFound_WhenMissing()
        {
            var svc = new Mock<ICardRequestService>();
            svc.Setup(s => s.GetCardRequestByIdAsync(2)).ReturnsAsync((CardRequest?)null);
            var controller = new CardRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<CardRequestController>());

            var res = await controller.GetCardRequest(2);
            Assert.IsType<NotFoundObjectResult>(res.Result);
        }

        [Fact]
        public async Task GetCardRequests_ReturnsNotFound_WhenEmpty()
        {
            var svc = new Mock<ICardRequestService>();
            svc.Setup(s => s.GetAllCardRequestsAsync()).ReturnsAsync(new List<CardRequest>());
            var controller = new CardRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<CardRequestController>());
            var res = await controller.GetCardRequests();
            Assert.IsType<NotFoundObjectResult>(res.Result);
        }

        [Fact]
        public async Task PostCardRequest_ReturnsCreated_WhenValid()
        {
            var svc = new Mock<ICardRequestService>();
            svc.Setup(s => s.AddCardRequestAsync(It.IsAny<CardRequest>())).ReturnsAsync((CardRequest r) => r);
            var bankSvc = new Mock<IBankAccountService>();
            bankSvc.Setup(b => b.GetBankAccountByIdAsync(It.IsAny<int>())).ReturnsAsync(new BankAccount { AccountStatus = AccountStatus.Active });
            var controller = new CardRequestController(svc.Object, bankSvc.Object, new NullLogger<CardRequestController>());
            var dto = new CardRequestDTO { AccountId = 5, CardBrand = CardBrand.Visa, CardType = CardType.Debit };
            var res = await controller.PostCardRequest(dto);
            Assert.IsType<CreatedAtActionResult>(res.Result);
        }

        [Fact]
        public async Task PutCardRequest_BadRequest_OnIdMismatch()
        {
            var svc = new Mock<ICardRequestService>();
            var controller = new CardRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<CardRequestController>());
            var dto = new CardRequestDTO { Id = 3 };
            var result = await controller.PutCardRequest(4, dto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ApproveCard_ReturnsOk_WhenRejected()
        {
            var svc = new Mock<ICardRequestService>();
            svc.Setup(s => s.ApproveCardRequestAsync(It.IsAny<CardApprovalDTO>())).ReturnsAsync((Card)null);
            var controller = new CardRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<CardRequestController>());
            var dto = new CardApprovalDTO { CardRequestId = 1, IsApproved = CardRequestStatus.Rejected };
            var res = await controller.ApproveCard(dto);
            var ok = Assert.IsType<OkObjectResult>(res);
            Assert.Equal("Card rejected successfully", ok.Value);
        }

        [Fact]
        public async Task ApproveCard_ReturnsOk_WhenApproved()
        {
            var svc = new Mock<ICardRequestService>();
            svc.Setup(s => s.ApproveCardRequestAsync(It.IsAny<CardApprovalDTO>())).ReturnsAsync(new Card());
            var controller = new CardRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<CardRequestController>());
            var dto = new CardApprovalDTO { CardRequestId = 1, IsApproved = CardRequestStatus.Approved };
            var res = await controller.ApproveCard(dto);
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async Task DeleteCardRequest_ReturnsNotFound_WhenMissing()
        {
            var svc = new Mock<ICardRequestService>();
            svc.Setup(s => s.GetCardRequestByIdAsync(9)).ReturnsAsync((CardRequest?)null);
            var controller = new CardRequestController(svc.Object, Mock.Of<IBankAccountService>(), new NullLogger<CardRequestController>());
            var res = await controller.DeleteCardRequest(9);
            Assert.IsType<NotFoundObjectResult>(res);
        }
    }
}
