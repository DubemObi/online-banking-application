using System;
using Banking.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace Banking.Tests.Services
{

    public class EmailServiceTests
    {
        [Fact]
        public void SendEmail_UsesSettingsAndClient()
        {
            var settings = new EmailSettings
            {
                SmtpServer = "smtp.test",
                SmtpPort = 25,
                SmtpUsername = "user",
                SmtpPassword = "pass"
            };
            var opts = Options.Create(settings);

            var smtpMock = new Mock<MailKit.Net.Smtp.ISmtpClient>();
            // for void methods we don't need explicit setup; Verify will catch calls


            var serviceMock = new Mock<EmailService>(opts) { CallBase = true };
            serviceMock.Protected()
                .Setup<MailKit.Net.Smtp.ISmtpClient>("CreateSmtpClient")
                .Returns(smtpMock.Object);

            var service = serviceMock.Object;
            service.SendEmail("test@me.com", "hello", "body");

            smtpMock.Verify(m => m.Connect(settings.SmtpServer, settings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            smtpMock.Verify(m => m.Authenticate(settings.SmtpUsername, settings.SmtpPassword, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            smtpMock.Verify(m => m.Disconnect(true, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }
    }
}
