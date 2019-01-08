using System.Collections.Generic;
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

        [Test]
        public async Task Post_WhenCalled_Should_Return_BadRequestResult_With_ModelState_Error_List()
        {
            // Arrange
            var expected = "Email is required";
            var contactMessageInputModel = new ContactMessageInputModel { Email = null, Message = "Wash hands", Name = "Tissue"};
            _sut.ModelState.AddModelError("Email", "Email is required");

            _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult{ IsSuccessful = true, Data = "success"});

            // Act
            var result = await _sut.Post(contactMessageInputModel) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Post_WhenCalled_Should_Return_BadRequestResult_With_SendEmail_Validation_Error_List()
        {
            // Arrange
            var expected = "Unable to send message.";
            var contactMessageInputModel = new ContactMessageInputModel { Email = "poo@toilet.com", Message = "Wash hands", Name = "Tissue"};

            _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult{ IsSuccessful = false, Data = "failed", Errors = new List<string> { "Unable to send message." } });

            // Act
            var result = await _sut.Post(contactMessageInputModel) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

    }
}