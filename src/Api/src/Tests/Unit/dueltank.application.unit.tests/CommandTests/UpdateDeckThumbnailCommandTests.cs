using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.application.Commands.UpdateDeckThumbnail;
using dueltank.application.Configuration;
using dueltank.application.Mappings.Profiles;
using dueltank.application.Models.Decks.Input;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.CommandTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class UpdateDeckThumbnailCommandTests
    {
        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration
            (
                cfg => { cfg.AddProfile(new DeckThumbnailProfile()); }
            );

            var mapper = config.CreateMapper();

            _settings = Substitute.For<IOptions<ApplicationSettings>>();
            _validator = Substitute.For<IValidator<DeckThumbnailInputModel>>();
            _userService = Substitute.For<IUserService>();
            _deckService = Substitute.For<IDeckService>();

            _sut = new UpdateDeckThumbnailCommandHandler(_settings, _validator, _userService, _deckService, mapper);
        }

        private UpdateDeckThumbnailCommandHandler _sut;
        private IUserService _userService;
        private IDeckService _deckService;
        private IOptions<ApplicationSettings> _settings;
        private IValidator<DeckThumbnailInputModel> _validator;

        [Test]
        public async Task Given_An_Invalid_DeckThumbnail_UpdateDeckThumbnail_Command_Should_Not_Be_Successful()
        {
            // Arrange
            var deckThumbnailInputModel = new DeckThumbnailInputModel();

            var command = new UpdateDeckThumbnailCommand {DeckThumbnail = deckThumbnailInputModel};
            _validator.Validate(Arg.Any<DeckThumbnailInputModel>()).Returns(new ValidationResult());

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Invalid_DeckThumbnail_UpdateDeckThumbnail_Command_Should_Return_A_List_Of_Errors()
        {
            // Arrange
            var deckThumbnailInputModel = new DeckThumbnailInputModel();

            var command = new UpdateDeckThumbnailCommand {DeckThumbnail = deckThumbnailInputModel};
            _validator.Validate(Arg.Any<DeckThumbnailInputModel>()).Returns(new ValidationResult
            {
                Errors = {new ValidationFailure("Validation property.", "Validation failed.")}
            });

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeEmpty();
        }

        [Test]
        public async Task
            Given_An_Valid_DeckThumbnail_If_User_Is_Not_OwnerUpdateDeckThumbnail_Command_Should_Be_Successful()
        {
            // Arrange
            var deckThumbnailInputModel = new DeckThumbnailInputModel
            {
                DeckId = 3242,
                Thumbnail = new byte[] {1, 2, 3},
                UserId = Guid.NewGuid().ToString()
            };

            var command = new UpdateDeckThumbnailCommand {DeckThumbnail = deckThumbnailInputModel};
            _validator.Validate(Arg.Any<DeckThumbnailInputModel>()).Returns(new ValidationResult());

            _userService.IsUserDeckOwner(Arg.Any<string>(), Arg.Any<long>()).Returns(true);
            _settings.Value.Returns(new ApplicationSettings {DeckThumbnailImageFolderPath = "c:/deck/thumbnail"});
            _deckService.SaveDeckThumbnail(Arg.Any<DeckThumbnail>()).Returns(3242);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_An_Valid_DeckThumbnail_UpdateDeckThumbnail_Command_Should_Be_Successful()
        {
            // Arrange
            var deckThumbnailInputModel = new DeckThumbnailInputModel
            {
                DeckId = 3242,
                Thumbnail = new byte[] {1, 2, 3},
                UserId = Guid.NewGuid().ToString()
            };

            var command = new UpdateDeckThumbnailCommand {DeckThumbnail = deckThumbnailInputModel};
            _validator.Validate(Arg.Any<DeckThumbnailInputModel>()).Returns(new ValidationResult());

            _userService.IsUserDeckOwner(Arg.Any<string>(), Arg.Any<long>()).Returns(true);
            _settings.Value.Returns(new ApplicationSettings {DeckThumbnailImageFolderPath = "c:/deck/thumbnail"});
            _deckService.SaveDeckThumbnail(Arg.Any<DeckThumbnail>()).Returns(3242);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}