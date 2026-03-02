using System.Collections.Generic;
using System.Threading.Tasks;
using Banking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using banking.Controllers;

namespace Banking.Tests.Controllers
{
    public class AuthControllerTests
    {
        private Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
        private Mock<SignInManager<ApplicationUser>> CreateSignInManagerMock(UserManager<ApplicationUser> userManager)
        {
            return new Mock<SignInManager<ApplicationUser>>(
                userManager,
                Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);
        }

        private IConfiguration GetConfiguration()
        {
            var dict = new Dictionary<string, string>
            {
                // key must be at least 128 bits for HS256
                ["Jwt:Key"] = "abcdefghijklmnopqrstuvwxyz123456",
                ["Jwt:Issuer"] = "issuer",
                ["Jwt:ExpireHours"] = "1"
            };
            return new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
        }

        // simple helper to provide a usable email service; tests don't rely on its behavior
        private EmailService CreateEmailService()
        {
            return new EmailService(Options.Create(new EmailSettings()));
        }

        [Fact]
        public async Task Register_ReturnsOk_OnSuccess()
        {
            var userMgrMock = CreateUserManagerMock();
            userMgrMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userMgrMock.Setup(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("token");

            var emailMock = new Mock<EmailService>(Options.Create(new EmailSettings()));
            emailMock.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var controller = new AuthController(userMgrMock.Object, CreateSignInManagerMock(userMgrMock.Object).Object,
                emailMock.Object, GetConfiguration());
            // setup Url and HttpContext to avoid null references during link generation
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext()
            };
            controller.ControllerContext.HttpContext.Request.Scheme = "http";
            var urlHelper = new Mock<Microsoft.AspNetCore.Mvc.IUrlHelper>();
            urlHelper.Setup(u => u.Action(It.IsAny<Microsoft.AspNetCore.Mvc.Routing.UrlActionContext>()))
                     .Returns("http://test/verify");
            controller.Url = urlHelper.Object;

            var dto = new RegisterUserDTO { Email = "a@b.com", Password = "pwd" };
            var res = await controller.Register(dto);
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_OnFailure()
        {
            var userMgrMock = CreateUserManagerMock();
            userMgrMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "err" }));
            var controller = new AuthController(userMgrMock.Object, CreateSignInManagerMock(userMgrMock.Object).Object,
                CreateEmailService(), GetConfiguration());
            var dto = new RegisterUserDTO { Email = "a@b.com", Password = "pwd" };
            var res = await controller.Register(dto);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_OnInvalid()
        {
            var userMgrMock = CreateUserManagerMock();
            var signInMock = CreateSignInManagerMock(userMgrMock.Object);
            signInMock.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
            var controller = new AuthController(userMgrMock.Object, signInMock.Object, CreateEmailService(), GetConfiguration());
            var res = await controller.Login(new AuthModel { Email = "x", Password = "y" });
            Assert.IsType<UnauthorizedObjectResult>(res);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithToken()
        {
            var user = new ApplicationUser { Email = "foo", Id = "id" };
            var userMgrMock = CreateUserManagerMock();
            userMgrMock.Setup(m => m.FindByEmailAsync("foo")).ReturnsAsync(user);
            userMgrMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });
            var signInMock = CreateSignInManagerMock(userMgrMock.Object);
            signInMock.Setup(s => s.PasswordSignInAsync("foo", "pwd", false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var controller = new AuthController(userMgrMock.Object, signInMock.Object, CreateEmailService(), GetConfiguration());
            var res = await controller.Login(new AuthModel { Email = "foo", Password = "pwd" });
            var ok = Assert.IsType<OkObjectResult>(res);
            // value should be an anonymous object containing a Token property
            Assert.NotNull(ok.Value);
            // use reflection to avoid dynamic binder issues
            var prop = ok.Value.GetType().GetProperty("Token");
            Assert.NotNull(prop);
            var tokenVal = prop.GetValue(ok.Value) as string;
            Assert.False(string.IsNullOrEmpty(tokenVal));
        }

        [Fact]
        public async Task Logout_ReturnsOk()
        {
            var userMgrMock = CreateUserManagerMock();
            var signInMock = CreateSignInManagerMock(userMgrMock.Object);
            signInMock.Setup(s => s.SignOutAsync()).Returns(Task.CompletedTask);
            var controller = new AuthController(userMgrMock.Object, signInMock.Object, CreateEmailService(), GetConfiguration());
            var res = await controller.Logout();
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenMissing()
        {
            var userMgrMock = CreateUserManagerMock();
            userMgrMock.Setup(m => m.FindByEmailAsync("a")).ReturnsAsync((ApplicationUser?)null);
            var controller = new AuthController(userMgrMock.Object, CreateSignInManagerMock(userMgrMock.Object).Object, CreateEmailService(), GetConfiguration());
            var res = await controller.DeleteUser("a");
            Assert.IsType<NotFoundObjectResult>(res);
        }

        [Fact]
        public async Task DeleteUser_ReturnsOk_WhenDeleted()
        {
            var user = new ApplicationUser { Email = "b" };
            var userMgrMock = CreateUserManagerMock();
            userMgrMock.Setup(m => m.FindByEmailAsync("b")).ReturnsAsync(user);
            userMgrMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);
            var controller = new AuthController(userMgrMock.Object, CreateSignInManagerMock(userMgrMock.Object).Object, CreateEmailService(), GetConfiguration());
            var res = await controller.DeleteUser("b");
            Assert.IsType<OkObjectResult>(res);
        }
    }
}
