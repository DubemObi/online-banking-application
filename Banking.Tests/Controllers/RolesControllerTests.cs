using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Banking.Controllers;

namespace Banking.Tests.Controllers
{
    public class RolesControllerTests
    {
        private Mock<RoleManager<IdentityRole>> CreateRoleManagerMock()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(store.Object, null, null, null, null);
        }

        private Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public void GetRoles_ReturnsOk()
        {
            var rm = CreateRoleManagerMock();
            rm.Setup(r => r.Roles).Returns(new List<IdentityRole> { new IdentityRole("a") }.AsQueryable());
            var controller = new RolesController(rm.Object, CreateUserManagerMock().Object);
            var res = controller.GetRoles();
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async Task GetRole_ReturnsNotFound_WhenMissing()
        {
            var rm = CreateRoleManagerMock();
            rm.Setup(r => r.FindByIdAsync("x")).ReturnsAsync((IdentityRole?)null);
            var controller = new RolesController(rm.Object, CreateUserManagerMock().Object);
            var res = await controller.GetRole("x");
            Assert.IsType<NotFoundObjectResult>(res);
        }

        [Fact]
        public async Task CreateRole_ReturnsOk_OnSuccess()
        {
            var rm = CreateRoleManagerMock();
            rm.Setup(r => r.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);
            var controller = new RolesController(rm.Object, CreateUserManagerMock().Object);
            var res = await controller.CreateRole("new");
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async Task UpdateRole_ReturnsNotFound_WhenRoleMissing()
        {
            var rm = CreateRoleManagerMock();
            rm.Setup(r => r.FindByIdAsync("id")).ReturnsAsync((IdentityRole?)null);
            var controller = new RolesController(rm.Object, CreateUserManagerMock().Object);
            var res = await controller.UpdateRole(new UpdateRoleModel { RoleId = "id", NewRoleName = "x" });
            Assert.IsType<NotFoundObjectResult>(res);
        }

        [Fact]
        public async Task DeleteRole_ReturnsNotFound_WhenMissing()
        {
            var rm = CreateRoleManagerMock();
            rm.Setup(r => r.FindByIdAsync("id")).ReturnsAsync((IdentityRole?)null);
            var controller = new RolesController(rm.Object, CreateUserManagerMock().Object);
            var res = await controller.DeleteRole("id");
            Assert.IsType<NotFoundObjectResult>(res);
        }

        [Fact]
        public async Task AssignRoleToUser_ReturnsNotFound_WhenUserMissing()
        {
            var rm = CreateRoleManagerMock();
            var um = CreateUserManagerMock();
            um.Setup(u => u.FindByIdAsync("uid")).ReturnsAsync((ApplicationUser?)null);
            var controller = new RolesController(rm.Object, um.Object);
            var res = await controller.AssignRoleToUser(new AssignRoleModel { UserId = "uid", RoleName = "r" });
            Assert.IsType<NotFoundObjectResult>(res);
        }
    }
}
