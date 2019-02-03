using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Commands.UpdateDeckThumbnail;
using dueltank.application.Commands.UploadYgoProDeck;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Decks.YgoProDeck;
using dueltank.core.Models.Db;
using dueltank.core.Models.YgoPro;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.CommandTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class UploadYgoProDeckCommandTests
    {
        private IValidator<UploadYgoProDeckCommand> commandValidator;
        IValidator<YgoProDeck> _ygoProDeckValidator;
        private IDeckService _deckService;
        private UploadYgoProDeckCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            commandValidator = Substitute.For<IValidator<UploadYgoProDeckCommand>>();
            _ygoProDeckValidator = Substitute.For<IValidator<YgoProDeck>>();

            _deckService = Substitute.For<IDeckService>();

            _sut = new UploadYgoProDeckCommandHandler(commandValidator, _ygoProDeckValidator, _deckService);
        }

        [Test]
        public async Task Given_An_Invalid_YgoProDeck_UploadYgoProDeck_Command_Should_Not_Be_Successful()
        {
            // Arrange
            var command = new UploadYgoProDeckCommand();
            commandValidator.Validate(Arg.Any<UploadYgoProDeckCommand>()).Returns(new ValidationResult{ Errors = { new ValidationFailure("DeckName", "Invalid deck name")}});

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Valid_YgoProDeck_That_Fails_Decks_Validation_UploadYgoProDeck_Command_Should_Not_Be_Successful()
        {
            // Arrange
            var deck = new StringBuilder();
            deck.Append("#created by ...");
            deck.AppendLine("#main");
            deck.AppendLine("16261341");
            deck.AppendLine("16261341");
            deck.AppendLine("16261341");
            deck.AppendLine("16261341");
            deck.AppendLine("#extra");
            deck.AppendLine("!side");

            var command = new UploadYgoProDeckCommand
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Test deck",
                Deck = deck.ToString()
            };
            commandValidator.Validate(Arg.Any<UploadYgoProDeckCommand>()).Returns(new ValidationResult());
            _ygoProDeckValidator.Validate(Arg.Any<YgoProDeck>()).Returns(new ValidationResult
            {
                Errors = {new ValidationFailure("CardCopyCount", "Only 3 copies of the card allowed.")}
            });

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Valid_YgoProDeck__Deck_Validation_Is_Successful_UploadYgoProDeck_Command_Should_Be_Successful()
        {
            // Arrange
            var deck = new StringBuilder();
            deck.Append("#created by ...");
            deck.AppendLine("#main");
            deck.AppendLine("16261341");
            deck.AppendLine("16261341");
            deck.AppendLine("16261341");
            deck.AppendLine("16261341");
            deck.AppendLine("#extra");
            deck.AppendLine("!side");

            var command = new UploadYgoProDeckCommand
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Test deck",
                Deck = deck.ToString()
            };
            commandValidator.Validate(Arg.Any<UploadYgoProDeckCommand>()).Returns(new ValidationResult());
            _ygoProDeckValidator.Validate(Arg.Any<YgoProDeck>()).Returns(new ValidationResult());

            _deckService.Add(Arg.Any<YgoProDeck>()).Returns(new Deck {Id = 3242342});

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

    }
}