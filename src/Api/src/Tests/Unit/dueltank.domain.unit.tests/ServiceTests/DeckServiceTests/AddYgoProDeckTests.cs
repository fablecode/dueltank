using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Models.YgoPro;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.domain.unit.tests.ServiceTests.DeckServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class AddYgoProDeckTests
    {
        private DeckService _sut;
        private IDeckTypeRepository _deckTypeRepository;
        private ICardRepository _cardRepository;
        private IDeckRepository _deckRepository;

        [SetUp]
        public void SetUp()
        {
            _deckRepository = Substitute.For<IDeckRepository>();
            _cardRepository = Substitute.For<ICardRepository>();
            _deckTypeRepository = Substitute.For<IDeckTypeRepository>();
            var deckCardRepository = Substitute.For<IDeckCardRepository>();

            _sut = new DeckService
            (
                _deckRepository,
                _cardRepository,
                _deckTypeRepository,
                deckCardRepository
            );
        }

        [Test]
        public async Task Given_A_Valid_YgoProDeck_If_Cards_Are_Found_Not_Found_Should_Return_Newly_Created_Deck()
        {
            // Arrange
            var ygoProDeck = new YgoProDeck
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Jinzo Deck",
                Description = "Machines are coming.",
                Main = new List<long> { 34230233, 34230233, 34230233, 99458769, 99458769, 99458769 },
                Extra = new List<long> { 31386180, 6832966, 16195942, 84013237, 94380860 },
                Side = new List<long>()
            };

            _deckTypeRepository.AllDeckTypes().Returns(new List<DeckType>
            {
                new DeckType
                {
                    Id = 23424,
                    Name = "Main",
                },
                new DeckType
                {
                    Id = 23424,
                    Name = "Extra",
                },
                new DeckType
                {
                    Id = 23424,
                    Name = "Side",
                }
            });

            // Act
            var result = await _sut.Add(ygoProDeck);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Given_A_Valid_YgoProDeck_Should_Persist_And_Return_Newly_Created_Deck()
        {
            // Arrange
            var ygoProDeck = new YgoProDeck
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Jinzo Deck",
                Description = "Machines are coming.",
                Main = new List<long> { 34230233, 34230233, 34230233, 99458769, 99458769, 99458769 },
                Extra = new List<long> { 31386180, 6832966, 16195942, 84013237, 94380860 },
                Side = new List<long>()
            };

            _deckTypeRepository.AllDeckTypes().Returns(new List<DeckType>
            {
                new DeckType
                {
                    Id = 23424,
                    Name = "Main",
                },
                new DeckType
                {
                    Id = 23424,
                    Name = "Extra",
                },
                new DeckType
                {
                    Id = 23424,
                    Name = "Side",
                }
            });

            _cardRepository.GetCardByNumber(Arg.Any<long>()).Returns(new Card());

            // Act
            var result = await _sut.Add(ygoProDeck);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Given_A_Valid_YgoProDeck_Should_Invoke_Add_Method_Once()
        {
            // Arrange
            var ygoProDeck = new YgoProDeck
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Jinzo Deck",
                Description = "Machines are coming.",
                Main = new List<long> { 34230233, 34230233, 34230233, 99458769, 99458769, 99458769 },
                Extra = new List<long> { 31386180, 6832966, 16195942, 84013237, 94380860 },
                Side = new List<long>()
            };

            _deckTypeRepository.AllDeckTypes().Returns(new List<DeckType>
            {
                new DeckType
                {
                    Id = 23424,
                    Name = "Main",
                },
                new DeckType
                {
                    Id = 23424,
                    Name = "Extra",
                },
                new DeckType
                {
                    Id = 23424,
                    Name = "Side",
                }
            });

            _cardRepository.GetCardByNumber(Arg.Any<long>()).Returns(new Card());

            // Act
            await _sut.Add(ygoProDeck);

            // Assert
            await _deckRepository.Received(1).Add(Arg.Any<Deck>());
        }

        [Test]
        public async Task Given_A_Valid_YgoProDeck_Should_Invoke_AllDeckTypes_Method_Once()
        {
            // Arrange
            var ygoProDeck = new YgoProDeck
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Jinzo Deck",
                Description = "Machines are coming.",
                Main = new List<long> { 34230233, 34230233, 34230233, 99458769, 99458769, 99458769 },
                Extra = new List<long> { 31386180, 6832966, 16195942, 84013237, 94380860 },
                Side = new List<long>()
            };

            _deckTypeRepository.AllDeckTypes().Returns(new List<DeckType>
            {
                new DeckType
                {
                    Id = 23424,
                    Name = "Main",
                },
                new DeckType
                {
                    Id = 23424,
                    Name = "Extra",
                },
                new DeckType
                {
                    Id = 23424,
                    Name = "Side",
                }
            });

            _cardRepository.GetCardByNumber(Arg.Any<long>()).Returns(new Card());

            // Act
            await _sut.Add(ygoProDeck);

            // Assert
            await _deckTypeRepository.Received(1).AllDeckTypes();
        }

        [Test]
        public async Task Given_A_Valid_YgoProDeck_Should_Invoke_GetCardByNumber_Method_Once()
        {
            // Arrange
            const int expected = 2;

            var ygoProDeck = new YgoProDeck
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Jinzo Deck",
                Description = "Machines are coming.",
                Main = new List<long> { 34230233 },
                Extra = new List<long> { 31386180 },
                Side = new List<long>()
            };

            _deckTypeRepository.AllDeckTypes().Returns(new List<DeckType>
            {
                new DeckType
                {
                    Id = 23424,
                    Name = "Main",
                },
                new DeckType
                {
                    Id = 23424,
                    Name = "Extra",
                },
                new DeckType
                {
                    Id = 23424,
                    Name = "Side",
                }
            });

            _cardRepository.GetCardByNumber(Arg.Any<long>()).Returns(new Card());

            // Act
            await _sut.Add(ygoProDeck);

            // Assert
            await _cardRepository.Received(expected).GetCardByNumber(Arg.Any<long>());
        }
    }
}