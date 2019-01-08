using System.Threading.Tasks;
using dueltank.api.Controllers;
using dueltank.api.Models.ContactUs.Input;
using dueltank.application.Commands;
using dueltank.tests.core;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.api.unit.tests.ControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ContactMessagesControllerTests
    {
        private IMediator _mediator;
        private ContactMessagesController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new ContactMessagesController(_mediator);
        }

        [Test]
        public async Task Post_WhenCalled_Should_Return_OkResult()
        {
            // Arrange
            var contactMessageInputModel = new ContactMessageInputModel{ Email = "poo@toilet.com", Message = "Wash hands", Name = "Tissue"};

            _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult{ IsSuccessful = true, Data = "success"});

            // Act
            var result = await _sut.Post(contactMessageInputModel);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

    }
}