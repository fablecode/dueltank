using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Commands.DeleteDeck;
using dueltank.application.Models.Cards.Input;
using dueltank.application.Models.Decks.Input;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.CommandTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DeleteDeckCommandTests
    {
        [SetUp]
        public void SetUp()
        {
            _userService = Substitute.For<IUserService>();
            _deckService = Substitute.For<IDeckService>();

            _sut = new DeleteDeckCommandHandler(_userService, _deckService);
        }

        private IUserService _userService;
        private IDeckService _deckService;
        private DeleteDeckCommandHandler _sut;

        [Test]
        public async Task Given_A_Deck_If_User_Is_Not_Owner_DeleteCommand_Should_Fail()
        {
            // Arrange
            var deck = new DeckInputModel
            {
                MainDeck = new List<CardInputModel>()
            };

            var command = new DeleteDeckCommand {Deck = deck};

            _userService.IsUserDeckOwner(Arg.Any<string>(), Arg.Any<long>()).Returns(false);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_A_Deck_If_User_Is_Not_Owner_Should_Not_Invoke_DeleteDeckByIdAndUserId()
        {
            // Arrange
            var deck = new DeckInputModel
            {
                MainDeck = new List<CardInputModel>()
            };

            var command = new DeleteDeckCommand {Deck = deck};

            _userService.IsUserDeckOwner(Arg.Any<string>(), Arg.Any<long>()).Returns(false);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            await _deckService.DidNotReceive().DeleteDeckByIdAndUserId(Arg.Any<string>(), Arg.Any<long>());
        }

        [Test]
        public async Task Given_A_Deck_If_User_Is_Not_Owner_Should_Return_Errors()
        {
            // Arrange
            var deck = new DeckInputModel
            {
                MainDeck = new List<CardInputModel>()
            };

            var command = new DeleteDeckCommand {Deck = deck};

            _userService.IsUserDeckOwner(Arg.Any<string>(), Arg.Any<long>()).Returns(false);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_A_Deck_If_User_Is_Owner_Should_Delete_Deck()
        {
            // Arrange
            var deck = new DeckInputModel
            {
                Id = 3242,
                MainDeck = new List<CardInputModel>()
            };

            var command = new DeleteDeckCommand {Deck = deck};

            _deckService.DeleteDeckByIdAndUserId(Arg.Any<string>(), Arg.Any<long>()).Returns(3242);
            _userService.IsUserDeckOwner(Arg.Any<string>(), Arg.Any<long>()).Returns(true);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            await _deckService.Received(1).DeleteDeckByIdAndUserId(Arg.Any<string>(), Arg.Any<long>());
        }
    }
}