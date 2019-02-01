using AutoFixture;
using AutoMapper;
using dueltank.application.Commands.UpdateDeck;
using dueltank.application.Mappings.Profiles;
using dueltank.application.Models.Cards.Input;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Decks;
using dueltank.core.Models.Db;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.unit.tests.CommandTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class UpdateDeckCommandTests
    {
        private UpdateDeckCommandHandler _sut;
        private Fixture _fixture;
        private IDeckService _deckService;
        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration
            (
                cfg =>
                {
                    cfg.AddProfile(new DeckProfile());

                }
            );

            var mapper = config.CreateMapper();

            _fixture = new Fixture();


            _deckService = Substitute.For<IDeckService>();
            _userService = Substitute.For<IUserService>();

            _sut = new UpdateDeckCommandHandler(new DeckValidator(), _deckService, mapper, _userService);
        }

        [Test]
        public async Task Given_An_Invalid_Deck_Update_Command_Should_Not_Be_Successful()
        {
            // Arrange
            var deck = new DeckInputModel
            {
                MainDeck = new List<CardInputModel>()
            };

            var command = new UpdateDeckCommand { Deck = deck };


            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Invalid_Deck_Update_Command_Should_Return_A_List_Of_Errors()
        {
            // Arrange
            var deck = new DeckInputModel
            {
                MainDeck = new List<CardInputModel>()
            };

            var command = new UpdateDeckCommand { Deck = deck };


            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_An_Valid_Deck_Update_Command_Should_Be_Successful()
        {
            // Arrange
            var deck = new DeckInputModel
            {
                Id = 3242,
                Name = "deck tester",
                UserId = Guid.NewGuid().ToString(),
                MainDeck = new List<CardInputModel>()
            };

            _fixture.RepeatCount = 40;

            deck.MainDeck =
                _fixture
                    .Build<CardInputModel>()
                    .With(c => c.BaseType, "monster")
                    .Without(c => c.Types)
                    .CreateMany()
                    .ToList();


            var command = new UpdateDeckCommand { Deck = deck };
            _userService.IsUserDeckOwner(Arg.Any<string>(), Arg.Any<long>()).Returns(true);
            _deckService.Update(Arg.Any<DeckModel>()).Returns(new Deck { Id = 23424 /*new deck id*/});

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_An_Valid_Deck_User_Is_Not_Owner_Update_Command_Should_Fail()
        {
            // Arrange
            var deck = new DeckInputModel
            {
                Id = 3242,
                Name = "deck tester",
                UserId = Guid.NewGuid().ToString(),
                MainDeck = new List<CardInputModel>()
            };

            _fixture.RepeatCount = 40;

            deck.MainDeck =
                _fixture
                    .Build<CardInputModel>()
                    .With(c => c.BaseType, "monster")
                    .Without(c => c.Types)
                    .CreateMany()
                    .ToList();


            var command = new UpdateDeckCommand { Deck = deck };
            _userService.IsUserDeckOwner(Arg.Any<string>(), Arg.Any<long>()).Returns(false);
            _deckService.Update(Arg.Any<DeckModel>()).Returns(new Deck { Id = 23424 /*new deck id*/});

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

    }
}