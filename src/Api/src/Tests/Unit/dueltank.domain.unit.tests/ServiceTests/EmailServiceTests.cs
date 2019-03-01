using dueltank.Domain.Model;
using dueltank.Domain.Service;
using dueltank.Domain.Smtp;
using dueltank.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.domain.unit.tests.ServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class EmailServiceTests
    {
        private ISmtpClient _smtpClient;
        private EmailService _sut;

        [SetUp]
        public void SetUp()
        {
            _smtpClient = Substitute.For<ISmtpClient>();

            _sut = new EmailService(_smtpClient);
        }

        [Test]
        public void Given_An_EmailMessage_Should_Invoke_Send_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var emailMessage = new EmailMessage();

            // Act
            _sut.Send(emailMessage);

            // Assert
            _smtpClient.Received(expected).Send(Arg.Any<EmailMessage>());
        }
    }
}