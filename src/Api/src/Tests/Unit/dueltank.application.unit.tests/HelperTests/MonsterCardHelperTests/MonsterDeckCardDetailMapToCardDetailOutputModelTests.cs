using System.Collections.Generic;
using dueltank.application.Helpers;
using dueltank.core.Models.Cards;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.HelperTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MonsterDeckCardDetailMapToCardDetailOutputModelTests
    {
        [Test]
        public void Given_A_DeckCardDetail_Should_Map_To_Monster_CardDetailOutputModel()
        {
            // Arrange
            var deckCardDetail = new DeckCardDetail
            {
                CategoryId = 23424,
                Category = "Monster",
                SubCategories = "Normal,Fairy"
            };

            // Act
            var result = MonsterCardHelper.MapToCardOutputModel(deckCardDetail);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void Given_A_DeckCardDetail_Should_Map_To_Monster_SubCategories_To_Types_()
        {
            // Arrange
            var expected = new List<string> { "Monster", "Normal", "Fairy" };

            var deckCardDetail = new DeckCardDetail
            {
                CategoryId = 23424,
                Category = "Monster",
                SubCategories = "Normal,Fairy"
            };

            // Act
            var result = MonsterCardHelper.MapToCardOutputModel(deckCardDetail);

            // Assert
            result.Types.Should().BeEquivalentTo(expected);
        }
    }
}