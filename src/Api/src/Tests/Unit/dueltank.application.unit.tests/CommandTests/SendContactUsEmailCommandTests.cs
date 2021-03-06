﻿using System;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Commands.SendContactUsEmail;
using dueltank.core.Services;
using dueltank.Domain.Configuration;
using dueltank.Domain.Model;
using dueltank.tests.core;
using FluentAssertions;
using MailKit;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.CommandTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class SendContactUsEmailCommandTests
    {
        [SetUp]
        public void SetUp()
        {
            _emailService = Substitute.For<IEmailService>();
            _emailConfiguration = Substitute.For<IEmailConfiguration>();

            _sut = new SendContactUsEmailCommandHandler(_emailService, _emailConfiguration);
        }

        private SendContactUsEmailCommandHandler _sut;
        private IEmailService _emailService;
        private IEmailConfiguration _emailConfiguration;

        [Test]
        public async Task
            Given_A_Valid_Command_If_Connection_To_EmailServer_Fails_Should_Handle_ServiceNotAuthenticated_Exception_And_Return_Errors()
        {
            // Arrange
            var command = new SendContactUsEmailCommand
            {
                Name = "bottom",
                Email = "poo@toilet.com",
                Message = "Flash the loo"
            };

            _emailConfiguration.SmtpUsername.Returns("dueltanco@gmail.com");

            _emailService.When(x => x.Send(Arg.Any<EmailMessage>()))
                .Do(x => throw new ServiceNotAuthenticatedException());

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeEmpty();
        }

        [Test]
        public async Task
            Given_A_Valid_Command_If_Connection_To_EmailServer_Fails_Should_Handle_ServiceNotConnected_Exception_And_Return_Errors()
        {
            // Arrange
            var command = new SendContactUsEmailCommand
            {
                Name = "bottom",
                Email = "poo@toilet.com",
                Message = "Flash the loo"
            };

            _emailConfiguration.SmtpUsername.Returns("dueltanco@gmail.com");

            _emailService.When(x => _emailService.Send(Arg.Any<EmailMessage>()))
                .Do(x => throw new ServiceNotConnectedException());

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeEmpty();
        }

        [Test]
        public void Given_A_Valid_Command_Should_Invoke_Send_Method_Once()
        {
            // Arrange
            var command = new SendContactUsEmailCommand
            {
                Name = "bottom",
                Email = "poo@toilet.com",
                Message = "Flash the loo"
            };

            _emailService.Send(Arg.Any<EmailMessage>());

            // Act
            _sut.Handle(command, CancellationToken.None);

            // Assert
            _emailService.Received(1).Send(Arg.Any<EmailMessage>());
        }

        [Test]
        public void Given_A_Valid_Command_Should_Send_Email_Successfully()
        {
            // Arrange
            var command = new SendContactUsEmailCommand
            {
                Name = "bottom",
                Email = "poo@toilet.com",
                Message = "Flash the loo"
            };

            _emailService.Send(Arg.Any<EmailMessage>());

            // Act
            Action act = () => _sut.Handle(command, CancellationToken.None);

            // Assert
            act.Should().NotThrow();
        }
    }
}