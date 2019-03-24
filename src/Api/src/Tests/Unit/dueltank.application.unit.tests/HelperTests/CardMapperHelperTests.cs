using System;
using AutoMapper;
using dueltank.application.Helpers;
using dueltank.application.Mappings.Profiles;
using dueltank.core.Models.Cards;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.HelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardMapperHelperTests
    {
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration
            (
                cfg => { cfg.AddProfile(new CardProfile()); }
            );

            _mapper = config.CreateMapper();
        }

        [Test]
        public void Given_An_Invalid_DeckCardDetail_Should_Throw_ArgumentOutOfRangeException()
        {
            // Arrange
            var deckCardSearch = new DeckCardDetail();

            // Act
            Action act = () => CardMapperHelper.MapToCardOutputModel(_mapper, deckCardSearch);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Given_A_Monster_DeckCardDetail_Should_Return_Monster_CardDetailOutputModel()
        {
            // Arrange

            var deckCardDetail = new DeckCardDetail
            {
                CategoryId = 23424,
                Category = "Monster",
                SubCategories = "Normal,Fairy",
                AttributeId = 43,
                Attribute = "Light",
                TypeId = 234,
                Type = "Ritual"
            };


            // Act
            var result = CardMapperHelper.MapToCardOutputModel(_mapper, deckCardDetail);

            // Assert
            result.Types.Should().Contain("Monster");
        }

        [Test]
        public void Given_A_Spell_DeckCardDetail_Should_Return_Monster_CardDetailOutputModel()
        {
            // Arrange

            var deckCardDetail = new DeckCardDetail
            {
                CategoryId = 23424,
                Category = "Spell",
                SubCategories = "Normal"
            };


            // Act
            var result = CardMapperHelper.MapToCardOutputModel(_mapper, deckCardDetail);

            // Assert
            result.Types.Should().Contain("Spell");
        }

        [Test]
        public void Given_A_Trap_DeckCardDetail_Should_Return_Monster_CardDetailOutputModel()
        {
            // Arrange

            var deckCardDetail = new DeckCardDetail
            {
                CategoryId = 23424,
                Category = "Trap",
                SubCategories = "Normal"
            };


            // Act
            var result = CardMapperHelper.MapToCardOutputModel(_mapper, deckCardDetail);

            // Assert
            result.Types.Should().Contain("Trap");
        }
    }
}