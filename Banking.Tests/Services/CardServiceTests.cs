using System.Collections.Generic;
using System.Threading.Tasks;
using Banking.Models;
using Moq;
using Xunit;

namespace Banking.Tests.Services
{
    // basic unit tests for the thin repository‑delegating CardService
    public class CardServiceTests
    {
        [Fact]
        public async Task AddCardAsync_CallsRepositoryAndReturnsCard()
        {
            var repoMock = new Mock<ICardRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<Card>())).Returns(Task.CompletedTask);

            var service = new CardService(repoMock.Object);

            var card = new Card { Id = 1 };
            var result = await service.AddCardAsync(card);

            Assert.Equal(card, result);
            repoMock.Verify(r => r.AddAsync(card), Times.Once);
        }

        [Fact]
        public async Task GetCardByIdAsync_ReturnsFromRepository()
        {
            var card = new Card { Id = 5 };
            var repoMock = new Mock<ICardRepository>();
            repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(card);

            var service = new CardService(repoMock.Object);
            var result = await service.GetCardByIdAsync(5);

            Assert.Equal(card, result);
        }

        [Fact]
        public async Task GetAllCardsAsync_ForwardsCall()
        {
            var list = new List<Card> { new Card() };
            var repoMock = new Mock<ICardRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var service = new CardService(repoMock.Object);
            var result = await service.GetAllCardsAsync();

            Assert.Equal(list, result);
        }

        [Fact]
        public async Task UpdateCardAsync_ForwardsCallAndReturnsCard()
        {
            var repoMock = new Mock<ICardRepository>();
            repoMock.Setup(r => r.UpdateAsync(It.IsAny<Card>())).Returns(Task.CompletedTask);
            var service = new CardService(repoMock.Object);

            var card = new Card { Id = 7 };
            var result = await service.UpdateCardAsync(7, card);

            Assert.Equal(card, result);
            repoMock.Verify(r => r.UpdateAsync(card), Times.Once);
        }

        [Fact]
        public async Task DeleteCardAsync_ReturnsFalseWhenMissing()
        {
            var repoMock = new Mock<ICardRepository>();
            repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Card?)null);
            var service = new CardService(repoMock.Object);

            var ok = await service.DeleteCardAsync(2);
            Assert.False(ok);
            repoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCardAsync_ReturnsTrueWhenExists()
        {
            var card = new Card { Id = 3 };
            var repoMock = new Mock<ICardRepository>();
            repoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(card);
            repoMock.Setup(r => r.DeleteAsync(3)).Returns(Task.CompletedTask);

            var service = new CardService(repoMock.Object);
            var ok = await service.DeleteCardAsync(3);

            Assert.True(ok);
            repoMock.Verify(r => r.DeleteAsync(3), Times.Once);
        }
    }
}
