using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.application.Mappings.Profiles;
using dueltank.application.Queries.CardByName;
using dueltank.core.Constants;
using dueltank.core.Models.Cards;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardByNameQueryTests
    {
        private ICardService _cardService;
        private CardByNameQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _cardService = Substitute.For<ICardService>();

            var config = new MapperConfiguration
            (
                cfg => { cfg.AddProfile(new CardProfile()); }
            );

            var mapper = config.CreateMapper();


            _sut = new CardByNameQueryHandler(_cardService, mapper);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task Given_An_Invalid_Card_Name_Should_Return_Null(string cardName)
        {
            // Arrange

            // Act
            var result = await _sut.Handle(new CardByNameQuery {Name = cardName}, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [TestCase("Blue-Eyes White Dragon")]
        public async Task Given_An_Valid_Card_Name_If_Card_Is_Not_Found_Should_Return_Null(string cardName)
        {
            // Arrange
            _cardService.GetCardByName(Arg.Any<string>()).Returns((CardSearch) null);

            // Act
            var result = await _sut.Handle(new CardByNameQuery {Name = cardName}, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
        [TestCase("Monster Reborn")]
        public async Task Given_An_Valid_Card_Name_If_Card_Is_Found_Should_Return_Card(string cardName)
        {
            // Arrange
            _cardService.GetCardByName(Arg.Any<string>()).Returns(new CardSearch
            {
                Category = CardConstants.SpellType,
                CategoryId = 2,
                Id = 234243,
                CardNumber = 90809,
                Name = cardName,
                Description = "Special Summon a monster from any graveyard.",
                SubCategories = "Normal"
            });

            // Act
            var result = await _sut.Handle(new CardByNameQuery {Name = cardName}, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
        }
    }
}