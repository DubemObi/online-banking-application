using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Banking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using banking.Controllers;

namespace Banking.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetUser_ReturnsOk_WhenFound()
        {
            var dto = new UserDTO { Id = "u1" };
            var svc = new Mock<IUserService>();
            svc.Setup(s => s.GetByIdAsync("u1")).ReturnsAsync(dto);
            var controller = new UserController(svc.Object, new NullLogger<UserController>());

            var res = await controller.GetUser("u1");
            var ok = Assert.IsType<OkObjectResult>(res.Result);
            Assert.Equal(dto, ok.Value);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenMissing()
        {
            var svc = new Mock<IUserService>();
            svc.Setup(s => s.GetByIdAsync("u2")).ReturnsAsync((UserDTO?)null);
            var controller = new UserController(svc.Object, new NullLogger<UserController>());

            var res = await controller.GetUser("u2");
            Assert.IsType<NotFoundObjectResult>(res.Result);
        }

        [Fact]
        public async Task GetUsers_ReturnsNotFound_WhenEmpty()
        {
            var svc = new Mock<IUserService>();
            svc.Setup(s => s.GetAllAsync(1, 100)).ReturnsAsync(new List<UserDTO>());
            var controller = new UserController(svc.Object, new NullLogger<UserController>());
            var res = await controller.GetUsers();
            Assert.IsType<NotFoundObjectResult>(res.Result);
        }

        [Fact]
        public async Task PutUser_BadRequest_OnIdMismatch()
        {
            var svc = new Mock<IUserService>();
            var controller = new UserController(svc.Object, new NullLogger<UserController>());
            var dto = new UserDTO { Id = "a" };
            var result = await controller.PutUser("b", dto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutUser_ReturnsOk_WhenUpdated()
        {
            var svc = new Mock<IUserService>();
            svc.Setup(s => s.UpdateAsync("u", It.IsAny<UserDTO>(), It.IsAny<ClaimsPrincipal>())).ReturnsAsync(true);
            var controller = new UserController(svc.Object, new NullLogger<UserController>());
            var dto = new UserDTO { Id = "u" };
            var res = await controller.PutUser("u", dto);
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async Task PutUser_NoContent_WhenUpdateFails()
        {
            var svc = new Mock<IUserService>();
            svc.Setup(s => s.UpdateAsync("u", It.IsAny<UserDTO>(), It.IsAny<ClaimsPrincipal>())).ReturnsAsync(false);
            var controller = new UserController(svc.Object, new NullLogger<UserController>());
            var dto = new UserDTO { Id = "u" };
            var res = await controller.PutUser("u", dto);
            Assert.IsType<NoContentResult>(res);
        }
    }
}
