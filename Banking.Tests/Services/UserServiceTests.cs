using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Banking.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Banking.Tests.Services
{
    public class UserServiceTests
    {
        private Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsPagedUsers()
        {
            var user1 = new ApplicationUser { Id = "1", Email = "a@a.com", FirstName = "A", LastName = "A" };
            var user2 = new ApplicationUser { Id = "2", Email = "b@b.com", FirstName = "B", LastName = "B" };
            var users = new List<ApplicationUser> { user1, user2 }.AsQueryable();

            var userMgrMock = CreateUserManagerMock();
            userMgrMock.Setup(m => m.Users).Returns(users);
            var service = new UserService(userMgrMock.Object);

            var result = await service.GetAllAsync(1, 10);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDto_WhenCurrentUserMatches()
        {
            var user = new ApplicationUser { Id = "99", Email = "x@x.com", FirstName = "X", LastName = "X" };
            var userMgrMock = CreateUserManagerMock();
            userMgrMock.Setup(m => m.FindByIdAsync("99")).ReturnsAsync(user);

            var service = new UserService(userMgrMock.Object);
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, "99")
            }));

            var dto = await service.GetByIdAsync("99");
            Assert.Equal("99", dto.Id);
        }

        [Fact]
        public async Task GetByIdAsync_Throws_WhenNotAuthorized()
        {
            var userMgrMock = CreateUserManagerMock();
            var service = new UserService(userMgrMock.Object);
            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await service.GetByIdAsync("abc"));
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenUserMissing()
        {
            var userMgrMock = CreateUserManagerMock();
            userMgrMock.Setup(m => m.FindByIdAsync("u")).ReturnsAsync((ApplicationUser?)null);
            var service = new UserService(userMgrMock.Object);
            var ok = await service.UpdateAsync("u", new UserDTO(), new ClaimsPrincipal());
            Assert.False(ok);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenUpdateSucceeds()
        {
            var user = new ApplicationUser { Id = "u" };
            var userMgrMock = CreateUserManagerMock();
            userMgrMock.Setup(m => m.FindByIdAsync("u")).ReturnsAsync(user);
            userMgrMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
            var service = new UserService(userMgrMock.Object);

            var ok = await service.UpdateAsync("u", new UserDTO { FirstName = "F", LastName = "L" }, new ClaimsPrincipal());
            Assert.True(ok);
        }
    }
}
