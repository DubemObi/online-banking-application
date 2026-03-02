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
    public class CardControllerTests
    {
        [Fact]
        public async Task GetCard_ReturnsOk_WhenFound()
        {
            var card = new Card { Id = 1, CardNumberHash = "h" };
            var svc = new Mock<ICardService>();
            svc.Setup(s => s.GetCardByIdAsync(1)).ReturnsAsync(card);
            var controller = new CardController(svc.Object, new NullLogger<CardController>());

            var result = await controller.GetCard(1);
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(card, ok.Value);
        }

        [Fact]
        public async Task GetCard_ReturnsNotFound_WhenMissing()
        {
            var svc = new Mock<ICardService>();
            svc.Setup(s => s.GetCardByIdAsync(2)).ReturnsAsync((Card?)null);
            var controller = new CardController(svc.Object, new NullLogger<CardController>());

            var result = await controller.GetCard(2);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetCards_ReturnsNotFound_WhenEmpty()
        {
            var svc = new Mock<ICardService>();
            svc.Setup(s => s.GetAllCardsAsync()).ReturnsAsync(new List<Card>());
            var controller = new CardController(svc.Object, new NullLogger<CardController>());
            var result = await controller.GetCards();
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostCard_ReturnsCreated_WhenValid()
        {
            var svc = new Mock<ICardService>();
            svc.Setup(s => s.AddCardAsync(It.IsAny<Card>())).ReturnsAsync((Card c) => c);
            var controller = new CardController(svc.Object, new NullLogger<CardController>());
            var dto = new CardDTO { CardNumber = "n", Last4Digits = "1234" };
            var res = await controller.PostCard(dto);
            var created = Assert.IsType<CreatedAtActionResult>(res.Result);
            Assert.IsType<Card>(created.Value);
        }

        [Fact]
        public async Task PutCard_BadRequest_OnIdMismatch()
        {
            var svc = new Mock<ICardService>();
            var controller = new CardController(svc.Object, new NullLogger<CardController>());
            var dto = new CardDTO { Id = 5 };
            var result = await controller.PutCard(6, dto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteCard_ReturnsNoContent_WhenExists()
        {
            var card = new Card { Id = 9 };
            var svc = new Mock<ICardService>();
            svc.Setup(s => s.GetCardByIdAsync(9)).ReturnsAsync(card);
            svc.Setup(s => s.DeleteCardAsync(9)).ReturnsAsync(true);
            var controller = new CardController(svc.Object, new NullLogger<CardController>());
            var res = await controller.DeleteCard(9);
            Assert.IsType<NoContentResult>(res);
        }

        [Fact]
        public async Task DeleteCard_ReturnsNotFound_WhenMissing()
        {
            var svc = new Mock<ICardService>();
            svc.Setup(s => s.GetCardByIdAsync(10)).ReturnsAsync((Card?)null);
            var controller = new CardController(svc.Object, new NullLogger<CardController>());
            var res = await controller.DeleteCard(10);
            Assert.IsType<NotFoundObjectResult>(res);
        }
    }
}
