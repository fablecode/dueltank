using AutoFixture;
using AutoMapper;
using dueltank.application.Commands.CreateDeck;
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
    public class CreateDeckCommandHandlerTests
    {
        private CreateDeckCommandHandler _sut;
        private Fixture _fixture;
        private IDeckService _deckService;

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
            _sut = new CreateDeckCommandHandler(new DeckValidator(), _deckService, mapper);
        }

        [Test]
        public async Task Given_An_Invalid_Deck_Create_Command_Should_Not_Be_Successful()
        {
            // Arrange
            var deck = new DeckInputModel
            {
                MainDeck = new List<CardInputModel>()
            };

            var command = new CreateDeckCommand {Deck = deck};


            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();

        }

        [Test]
        public async Task Given_An_Invalid_Deck_Create_Command_Should_Return_A_List_Of_Errors()
        {
            // Arrange
            var deck = new DeckInputModel
            {
                MainDeck = new List<CardInputModel>()
            };

            var command = new CreateDeckCommand {Deck = deck};


            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_An_Valid_Deck_Create_Command_Should_Be_Successful()
        {
            // Arrange
            var deck = new DeckInputModel
            {
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


            var command = new CreateDeckCommand { Deck = deck};
            _deckService.Add(Arg.Any<DeckModel>()).Returns(new Deck {Id = 23424 /*new deck id*/});

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_An_Valid_Deck_Create_Command_Should_Be_Successful_And_No_Errors_Returned()
        {
            // Arrange
            var deck = new DeckInputModel
            {
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


            var command = new CreateDeckCommand { Deck = deck};
            _deckService.Add(Arg.Any<DeckModel>()).Returns(new Deck {Id = 23424 /*new deck id*/});

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Errors.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task Given_An_Valid_Deck_Create_Command_Should_Be_Successful_And_New_DeckId_Returned()
        {
            // Arrange
            const int expected = 23424;

            var deck = new DeckInputModel
            {
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


            var command = new CreateDeckCommand { Deck = deck};
            _deckService.Add(Arg.Any<DeckModel>()).Returns(new Deck { Id = 23424 /*new deck id*/});

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            dynamic dyn = result.Data;
            int newDeckId = int.Parse(dyn.GetType().GetProperty("deckId").GetValue(dyn, null).ToString());
            newDeckId.Should().Be(expected);
        }

        [Test]
        public async Task Given_An_Valid_Deck_Create_Command_Should_Invoke_DeckService_Add_Method_Once()
        {
            // Arrange
            var deck = new DeckInputModel
            {
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


            var command = new CreateDeckCommand { Deck = deck};
            _deckService.Add(Arg.Any<DeckModel>()).Returns(new Deck {Id = 23424 /*new deck id*/});

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            await _deckService.Received(1).Add(Arg.Any<DeckModel>());
        }
    }
}