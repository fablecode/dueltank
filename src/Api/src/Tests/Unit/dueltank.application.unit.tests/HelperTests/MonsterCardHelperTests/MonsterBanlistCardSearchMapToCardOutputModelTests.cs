using System.Collections.Generic;
using dueltank.application.Helpers;
using dueltank.core.Models.Search.Banlists;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.HelperTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MonsterBanlistCardSearchMapToCardOutputModelTests
    {
        [Test]
        public void Given_A_BanlistCardSearch_Should_Map_To_Monster_CardOutputModel()
        {
            // Arrange
            var cardSearch = new BanlistCardSearch
            {
                CategoryId = 23424,
                Category = "Monster",
                SubCategories = "Normal,Fairy",
            };

            // Act
            var result = MonsterCardHelper.MapToCardOutputModel(cardSearch);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void Given_A_BanlistCardSearch_Should_Map_To_Monster_SubCategories_To_Types_()
        {
            // Arrange
            var expected = new List<string> { "Monster", "Normal", "Fairy" };

            var deckCardDetail = new BanlistCardSearch
            {
                CategoryId = 23424,
                Category = "Monster",
                SubCategories = "Normal,Fairy",
            };

            // Act
            var result = MonsterCardHelper.MapToCardOutputModel(deckCardDetail);

            // Assert
            result.Types.Should().BeEquivalentTo(expected);
        }
    }
}