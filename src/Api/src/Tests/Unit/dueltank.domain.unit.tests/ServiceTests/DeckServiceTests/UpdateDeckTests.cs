using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;
using dueltank.core.Models.DeckDetails;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.Domain.SystemIO;

namespace dueltank.domain.unit.tests.ServiceTests.DeckServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class UpdateDeckTests
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
            var deckFileSystem = Substitute.For<IDeckFileSystem>();

            _sut = new DeckService
            (
                _deckRepository,
                _cardRepository,
                _deckTypeRepository,
                deckCardRepository,
                deckFileSystem
            );
        }

        [Test]
        public async Task Given_A_Valid_Deck_If_Cards_Are_Found_Not_Found_Should_Return_Updated_Deck()
        {
            // Arrange
            var updatedDeck = new DeckModel
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Jinzo Deck",
                Description = "Machines are coming.",
                MainDeck = new List<CardModel>
                {
                    new CardModel{ Id = 34230233 },
                    new CardModel { Id = 34230233 },
                    new CardModel { Id = 34230233 },
                    new CardModel { Id = 99458769 },
                    new CardModel { Id = 99458769 },
                    new CardModel { Id = 99458769 }
                },
                ExtraDeck = new List<CardModel>
                {
                    new CardModel{ Id = 31386180 },
                    new CardModel { Id = 6832966 },
                    new CardModel { Id = 16195942 },
                    new CardModel { Id = 84013237 },
                    new CardModel { Id = 94380860 },
                },
                SideDeck = new List<CardModel>()
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

            _deckRepository.GetDeckById(Arg.Any<long>()).Returns(new DeckDetail
            {
                Id = 2342342,
                UserId = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            });


            _deckRepository.Update(Arg.Any<Deck>()).Returns(new Deck());

            // Act
            var result = await _sut.Update(updatedDeck);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Given_A_Valid_Deck_Should_Update_And_Return_Updated_Deck()
        {
            // Arrange
            var updatedDeck = new DeckModel
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Jinzo Deck",
                Description = "Machines are coming.",
                MainDeck = new List<CardModel>
                {
                    new CardModel{ Id = 34230233 },
                    new CardModel { Id = 34230233 },
                    new CardModel { Id = 34230233 },
                    new CardModel { Id = 99458769 },
                    new CardModel { Id = 99458769 },
                    new CardModel { Id = 99458769 }
                },
                ExtraDeck = new List<CardModel>
                {
                    new CardModel{ Id = 31386180 },
                    new CardModel { Id = 6832966 },
                    new CardModel { Id = 16195942 },
                    new CardModel { Id = 84013237 },
                    new CardModel { Id = 94380860 },
                },
                SideDeck = new List<CardModel>()
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

            _deckRepository.GetDeckById(Arg.Any<long>()).Returns(new DeckDetail
            {
                Id = 2342342,
                UserId = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            });


            _deckRepository.Update(Arg.Any<Deck>()).Returns(new Deck());

            // Act
            var result = await _sut.Update(updatedDeck);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Given_A_Valid_Deck_Should_Invoke_Updated_Method_Once()
        {
            // Arrange
            var updatedDeck = new DeckModel
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Jinzo Deck",
                Description = "Machines are coming.",
                MainDeck = new List<CardModel>
                {
                    new CardModel{ Id = 34230233 },
                    new CardModel { Id = 34230233 },
                    new CardModel { Id = 34230233 },
                    new CardModel { Id = 99458769 },
                    new CardModel { Id = 99458769 },
                    new CardModel { Id = 99458769 }
                },
                ExtraDeck = new List<CardModel>
                {
                    new CardModel{ Id = 31386180 },
                    new CardModel { Id = 6832966 },
                    new CardModel { Id = 16195942 },
                    new CardModel { Id = 84013237 },
                    new CardModel { Id = 94380860 },
                },
                SideDeck = new List<CardModel>()
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

            _deckRepository.GetDeckById(Arg.Any<long>()).Returns(new DeckDetail
            {
                Id = 2342342,
                UserId = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            });


            _deckRepository.Update(Arg.Any<Deck>()).Returns(new Deck());

            // Act
            await _sut.Update(updatedDeck);

            // Assert
            await _deckRepository.Received(1).Update(Arg.Any<Deck>());
        }

        [Test]
        public async Task Given_A_Valid_Deck_Should_Invoke_AllDeckTypes_Method_Once()
        {
            // Arrange
            var updatedDeck = new DeckModel
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Jinzo Deck",
                Description = "Machines are coming.",
                MainDeck = new List<CardModel>
                {
                    new CardModel{ Id = 34230233 },
                    new CardModel { Id = 34230233 },
                    new CardModel { Id = 34230233 },
                    new CardModel { Id = 99458769 },
                    new CardModel { Id = 99458769 },
                    new CardModel { Id = 99458769 }
                },
                ExtraDeck = new List<CardModel>
                {
                    new CardModel{ Id = 31386180 },
                    new CardModel { Id = 6832966 },
                    new CardModel { Id = 16195942 },
                    new CardModel { Id = 84013237 },
                    new CardModel { Id = 94380860 },
                },
                SideDeck = new List<CardModel>()
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

            _deckRepository.GetDeckById(Arg.Any<long>()).Returns(new DeckDetail
            {
                Id = 2342342,
                UserId = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            });


            _deckRepository.Update(Arg.Any<Deck>()).Returns(new Deck());

            // Act
            await _sut.Update(updatedDeck);

            // Assert
            await _deckTypeRepository.Received(1).AllDeckTypes();
        }

        [Test]
        public async Task Given_A_Valid_Deck_Should_Invoke_GetCardById_Method_Twice()
        {
            // Arrange
            const int expected = 2;

            var updatedDeck = new DeckModel
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "Jinzo Deck",
                Description = "Machines are coming.",
                MainDeck = new List<CardModel>
                {
                    new CardModel{ Id = 34230233 },
                },
                ExtraDeck = new List<CardModel>
                {
                    new CardModel{ Id = 31386180 },
                },
                SideDeck = new List<CardModel>()
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


            _deckRepository.GetDeckById(Arg.Any<long>()).Returns(new DeckDetail
            {
                Id = 2342342,
                UserId = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            });


            _deckRepository.Update(Arg.Any<Deck>()).Returns(new Deck());

            // Act
            await _sut.Update(updatedDeck);

            // Assert
            await _cardRepository.Received(expected).GetCardById(Arg.Any<long>());
        }
    }
}