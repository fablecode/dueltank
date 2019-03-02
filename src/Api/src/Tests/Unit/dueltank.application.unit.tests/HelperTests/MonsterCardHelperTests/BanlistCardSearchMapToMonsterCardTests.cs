using System.Collections.Generic;
using System.Linq;
using dueltank.application.Helpers;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Search.Banlists;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.HelperTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class BanlistCardSearchMapToMonsterCardTests
    {
        [Test]
        public void Given_A_BanlistCardSearch_Should_Map_To_Monster_Card()
        {
            // Arrange
            var cardSearch = new CardSearch
            {
                CategoryId = 23424,
                Category = "Monster",
                SubCategories = "Normal,Fairy",
            };

            // Act
            var result = MonsterCardHelper.MapToMonsterCard(cardSearch);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void Given_A_BanlistCardSearch_Should_Map_To_Monster_SubCategories_To_CardSubCategory()
        {
            // Arrange
            var expected = new List<string> { "Normal", "Fairy" };

            var deckCardDetail = new BanlistCardSearch
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
            var result = MonsterCardHelper.MapToMonsterCard(deckCardDetail);

            // Assert
            result.CardSubCategory.Select(c => c.SubCategory.Name).Should().BeEquivalentTo(expected);
        }
    }
}