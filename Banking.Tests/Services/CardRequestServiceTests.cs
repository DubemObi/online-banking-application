using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Banking.Models;
using Moq;
using Xunit;

namespace Banking.Tests.Services
{
    public class CardRequestServiceTests
    {
        [Fact]
        public async Task AddCardRequestAsync_CallsRepositoryAndReturns()
        {
            var repoMock = new Mock<ICardRequestRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<CardRequest>())).Returns(Task.CompletedTask);

            var service = new CardRequestService(repoMock.Object, Mock.Of<ICardService>(), Mock.Of<IBankAccountService>());
            var req = new CardRequest { Id = 1 };
            var result = await service.AddCardRequestAsync(req);

            Assert.Equal(req, result);
            repoMock.Verify(r => r.AddAsync(req), Times.Once);
        }

        [Fact]
        public async Task ApproveCardRequestAsync_Throws_WhenRequestNotFound()
        {
            var repoMock = new Mock<ICardRequestRepository>();
            repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((CardRequest?)null);

            var service = new CardRequestService(repoMock.Object, Mock.Of<ICardService>(), Mock.Of<IBankAccountService>());
            await Assert.ThrowsAsync<Exception>(async () =>
                await service.ApproveCardRequestAsync(new CardApprovalDTO { CardRequestId = 5, IsApproved = CardRequestStatus.Approved }));
        }

        [Fact]
        public async Task ApproveCardRequestAsync_Throws_WhenAlreadyProcessed()
        {
            var existing = new CardRequest { Id = 2, Status = CardRequestStatus.Approved };
            var repoMock = new Mock<ICardRequestRepository>();
            repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(existing);

            var service = new CardRequestService(repoMock.Object, Mock.Of<ICardService>(), Mock.Of<IBankAccountService>());
            await Assert.ThrowsAsync<Exception>(async () =>
                await service.ApproveCardRequestAsync(new CardApprovalDTO { CardRequestId = 2, IsApproved = CardRequestStatus.Approved }));
        }

        [Fact]
        public async Task ApproveCardRequestAsync_Rejects_ReturnsNull()
        {
            var existing = new CardRequest { Id = 3, Status = CardRequestStatus.Pending, AccountId = 10, UserId = "7", CardBrand = CardBrand.Visa, CardType = CardType.Debit };
            var repoMock = new Mock<ICardRequestRepository>();
            repoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(existing);
            repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            var bankMock = new Mock<IBankAccountService>();
            bankMock.Setup(b => b.GetBankAccountByIdAsync(10)).ReturnsAsync(new BankAccount { AccountId = 10, AccountStatus = AccountStatus.Active, AccountName = "A", AccountNumber = "N", RowVersion = new byte[] { 0 } });

            var service = new CardRequestService(repoMock.Object, Mock.Of<ICardService>(), bankMock.Object);
            var result = await service.ApproveCardRequestAsync(new CardApprovalDTO { CardRequestId = 3, IsApproved = CardRequestStatus.Rejected });

            Assert.Null(result);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ApproveCardRequestAsync_Approves_CreatesCard()
        {
            var existing = new CardRequest { Id = 4, Status = CardRequestStatus.Pending, AccountId = 11, UserId = "8", CardBrand = CardBrand.MasterCard, CardType = CardType.Credit };
            var repoMock = new Mock<ICardRequestRepository>();
            repoMock.Setup(r => r.GetByIdAsync(4)).ReturnsAsync(existing);
            repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            var bankMock = new Mock<IBankAccountService>();
            bankMock.Setup(b => b.GetBankAccountByIdAsync(11)).ReturnsAsync(new BankAccount { AccountId = 11, AccountStatus = AccountStatus.Active, AccountName = "A", AccountNumber = "N", RowVersion = new byte[] { 0 } });
            var cardMock = new Mock<ICardService>();
            cardMock.Setup(c => c.AddCardAsync(It.IsAny<Card>())).ReturnsAsync((Card c) => c);

            var service = new CardRequestService(repoMock.Object, cardMock.Object, bankMock.Object);
            var dto = new CardApprovalDTO { CardRequestId = 4, IsApproved = CardRequestStatus.Approved };
            var card = await service.ApproveCardRequestAsync(dto);

            Assert.NotNull(card);
            cardMock.Verify(c => c.AddCardAsync(It.IsAny<Card>()), Times.Once);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCardRequestByIdAsync_Forwards()
        {
            var req = new CardRequest { Id = 9 };
            var repoMock = new Mock<ICardRequestRepository>();
            repoMock.Setup(r => r.GetByIdAsync(9)).ReturnsAsync(req);
            var service = new CardRequestService(repoMock.Object, Mock.Of<ICardService>(), Mock.Of<IBankAccountService>());
            Assert.Equal(req, await service.GetCardRequestByIdAsync(9));
        }

        [Fact]
        public async Task GetAllCardRequestsAsync_Forwards()
        {
            var list = new List<CardRequest>();
            var repoMock = new Mock<ICardRequestRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);
            var service = new CardRequestService(repoMock.Object, Mock.Of<ICardService>(), Mock.Of<IBankAccountService>());
            Assert.Equal(list, await service.GetAllCardRequestsAsync());
        }

        [Fact]
        public async Task UpdateCardRequestAsync_Forwards()
        {
            var repoMock = new Mock<ICardRequestRepository>();
            repoMock.Setup(r => r.UpdateAsync(It.IsAny<CardRequest>())).Returns(Task.CompletedTask);
            var service = new CardRequestService(repoMock.Object, Mock.Of<ICardService>(), Mock.Of<IBankAccountService>());
            var req = new CardRequest { Id = 12 };
            Assert.Equal(req, await service.UpdateCardRequestAsync(12, req));
            repoMock.Verify(r => r.UpdateAsync(req), Times.Once);
        }

        [Fact]
        public async Task DeleteCardRequestAsync_ReturnsFalseWhenMissing()
        {
            var repoMock = new Mock<ICardRequestRepository>();
            repoMock.Setup(r => r.GetByIdAsync(20)).ReturnsAsync((CardRequest?)null);
            var service = new CardRequestService(repoMock.Object, Mock.Of<ICardService>(), Mock.Of<IBankAccountService>());
            Assert.False(await service.DeleteCardRequestAsync(20));
        }

        [Fact]
        public async Task DeleteCardRequestAsync_ReturnsTrueWhenExists()
        {
            var repoMock = new Mock<ICardRequestRepository>();
            repoMock.Setup(r => r.GetByIdAsync(21)).ReturnsAsync(new CardRequest { Id = 21 });
            repoMock.Setup(r => r.DeleteAsync(21)).Returns(Task.CompletedTask);
            var service = new CardRequestService(repoMock.Object, Mock.Of<ICardService>(), Mock.Of<IBankAccountService>());
            Assert.True(await service.DeleteCardRequestAsync(21));
            repoMock.Verify(r => r.DeleteAsync(21), Times.Once);
        }
    }
}
